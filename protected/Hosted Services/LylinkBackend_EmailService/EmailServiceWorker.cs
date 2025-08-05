using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkBackend_DatabaseAccessLayer.Services;
using LylinkBackend_EmailService.Models;
using LylinkBackend_EmailService.Services;
using Microsoft.Extensions.Options;
using System.Text;

namespace LylinkBackend_EmailService
{
    public class EmailServiceWorker(IServiceProvider serviceProvider, IOptions<Email> emailOptions) : BackgroundService
    {
        private readonly TimeSpan targetTime = new(0, 00, 0);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
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

                IEnumerable<VisitAnalytic> analytics = visitAnalytics.GetAllVisitorAnalytics();

                string body = GenerateEmailBody(analytics);

                EmailAttachment csvAttachment = new EmailAttachment
                {
                    AttachmentData = Encoding.ASCII.GetBytes(CreateCsvForVisitors(analytics)),
                    FileName = "raw_analytics.csv"
                };

                await emailService.SendEmail(analyticsEmailRecipient, "Site Visitor Analytics", body, [csvAttachment]);

                visitAnalytics.DropAllVisitorAnalytics();
            }
        }

        private static string GenerateEmailBody(IEnumerable<VisitAnalytic> analytics)
        {
            var pageVisits = analytics
                .GroupBy(a => a.SlugVisited)
                .Where(g => g.Key != null)
                .Select(g => new { Page = g.Key, VisitCount = g.Count() })
                .OrderByDescending(x => x.VisitCount);

            var visitorVisits = analytics
                .GroupBy(a => a.VisitorId)
                .Where(g => g.Key != null)
                .Select(g => new
                {
                    Visitor = g.Key,
                    VisitCount = g.Count(),
                    MostVisitedPages = g.GroupBy(v => v.SlugVisited)
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
            emailBody.AppendLine("</body>");
            emailBody.AppendLine("</html>");


            return emailBody.ToString();
        }

        private static string CreateCsvForVisitors(IEnumerable<VisitAnalytic> analytics)
        {
            List<string> csv = ["Id,VisitorId,SlugVisited,SlugGiven,VisitedOn"];

            foreach (VisitAnalytic analytic in analytics)
            {
                csv.Add($"{analytic.Id},{analytic.VisitorId},{analytic.SlugVisited},{analytic.SlugGiven},{analytic.VisitedOn}");
            }

            return string.Join("\n", csv);
        }
    }
}
