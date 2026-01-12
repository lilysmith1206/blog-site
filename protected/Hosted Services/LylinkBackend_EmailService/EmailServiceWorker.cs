using LylinkBackend.Repositories.Analytics;
using LylinkBackend.Repositories.Models;
using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkBackend_EmailService.Models;
using LylinkBackend_EmailService.Services;
using Microsoft.Extensions.Options;
using System.Text;

namespace LylinkBackend_EmailService;

public class EmailServiceWorker(IServiceProvider serviceProvider, IOptions<EmailOptions> emailOptions) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (emailOptions.Value.EasternTimeSent is null)
            throw new ArgumentNullException("Email send time is null.");

        var easternTimeSent = emailOptions.Value.EasternTimeSent.Value;
        TimeSpan targetTime = new(easternTimeSent.Hour, easternTimeSent.Minute, easternTimeSent.Second);

        while (stoppingToken.IsCancellationRequested == false)
        {
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime currentEastern = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

            DateTime nextRun = currentEastern.Date.Add(targetTime);

            if (currentEastern > nextRun)
            {
                nextRun = nextRun.AddDays(1);
            }

            TimeSpan delay = nextRun - currentEastern;

            try
            {
                await Task.Delay(delay, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }

            using var scope = serviceProvider.CreateScope();
            IEmailService emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            string? analyticsEmailRecipient = emailOptions.Value.AnalyticsEmailRecipient;

            if (analyticsEmailRecipient == null)
            {
                Console.WriteLine("EMAIL CANNOT BE SENT");

                return;
            }

            IVisitAnalyticsRepository visitAnalytics = scope.ServiceProvider.GetRequiredService<IVisitAnalyticsRepository>();

            var analytics = visitAnalytics.GetAllVisitorAnalytics();

            string body = GenerateEmailBody(analytics);

            EmailAttachment successVisitCsv = new EmailAttachment
            {
                AttachmentData = Encoding.ASCII.GetBytes(CreateCsvForVisitors(analytics.SuccessVisits)),
                FileName = "raw_visitor_analytics.csv"
            };

            EmailAttachment failedVisitCsv = new EmailAttachment
            {
                AttachmentData = Encoding.ASCII.GetBytes(CreateCsvForFailedVisits(analytics.FailedVisit)),
                FileName = "raw_failed_analytics.csv"
            };

            bool successfulEmailSent = await emailService.SendEmail(analyticsEmailRecipient, "Site Visitor Analytics", body, [successVisitCsv, failedVisitCsv]);

            if (successfulEmailSent)
            {
                visitAnalytics.DropAllVisitorAnalytics();
            }
        }
    }

    private static string GenerateEmailBody(Analytics analytics)
    {
        var pageVisits = analytics.SuccessVisits
            .GroupBy(a => a.VisitedSlug)
            .Where(g => g.Key != null)
            .Select(g => new { Page = g.Key, VisitCount = g.Count() })
            .OrderByDescending(x => x.VisitCount);

        var visitorVisits = analytics.SuccessVisits
            .GroupBy(a => a.SessionId)
            .Where(g => g.Key != null)
            .Select(g => new
            {
                Visitor = g.Key,
                VisitCount = g.Count(),
                MostVisitedPages = g.GroupBy(v => v.VisitedSlug)
                                    .Where(vg => vg.Key != null)
                                    .Select(vg => new { Page = vg.Key, VisitCount = vg.Count() })
                                    .OrderByDescending(v => v.VisitCount)
                                    .Take(3)
                                    .ToList()
            })
            .OrderByDescending(x => x.VisitCount);

        StringBuilder emailBody = new();

        emailBody.AppendLine("<html>");
        emailBody.AppendLine("<head><style>body { font-family: Arial, sans-serif; }</style></head>");
        emailBody.AppendLine("<body>");
        emailBody.AppendLine("<h1>Daily Visit Analytics Report</h1>");
        emailBody.AppendLine("<h2>Most Visited Pages:</h2>");
        emailBody.AppendLine("<ul>");

        foreach (var page in pageVisits)
        {
            emailBody.AppendLine($"<li>{page.Page}: {page.VisitCount} visits</li>");
        }

        emailBody.AppendLine("</ul>");
        emailBody.AppendLine("<h2>Top Visitors:</h2>");
        emailBody.AppendLine("<ul>");

        foreach (var visitor in visitorVisits.Take(2))
        {
            emailBody.AppendLine($"<li>{visitor.Visitor}: {visitor.VisitCount} visits");
            emailBody.AppendLine("<ul>");
            foreach (var page in visitor.MostVisitedPages)
            {
                emailBody.AppendLine($"<li>{page.Page}: {page.VisitCount} visits</li>");
            }
            emailBody.AppendLine("</ul>");
            emailBody.AppendLine("</li>");
        }

        emailBody.AppendLine("</ul>");

        emailBody.AppendLine("<h2>Rejected Page Visits</h2>");
        emailBody.AppendLine("<ul>");

        foreach (var failedVisit in analytics.FailedVisit)
        {
            emailBody.AppendLine($"<li>{failedVisit.AttemptedSlug}");
        }

        emailBody.AppendLine("</ul>");
        emailBody.AppendLine("</body>");
        emailBody.AppendLine("</html>");


        return emailBody.ToString();
    }

    private static string CreateCsvForVisitors(IEnumerable<VisitAnalytic> analytics)
    {
        List<string> csv = ["Id,SessionId,VisitedSlug,VisitedOn"];

        foreach (VisitAnalytic analytic in analytics)
        {
            csv.Add($"{analytic.Id},{analytic.SessionId},{analytic.VisitedSlug},{analytic.DateCreated}");
        }

        return string.Join("\n", csv);
    }

    private static string CreateCsvForFailedVisits(IEnumerable<FailedVisitAnalytic> analytics)
    {
        List<string> csv = ["Id,SessionId,AttemptedSlug,RedirectedSlug,VisitedOn"];

        foreach (var analytic in analytics)
        {
            csv.Add($"{analytic.Id},{analytic.SessionId},{analytic.AttemptedSlug},{analytic.RedirectedSlug},{analytic.DateCreated}");
        }

        return string.Join("\n", csv);
    }
}
