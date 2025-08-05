using Microsoft.Extensions.Options;
using Resend;

namespace LylinkBackend_EmailService.Services
{
    public class EmailService(IResend resendClient, IOptions<Models.Email> emailOptions) : IEmailService
    {
        public async Task SendEmail(string toAddress, string subject, string body, IEnumerable<Models.EmailAttachment>? attachments = null)
        {
            try
            {
                IEnumerable<EmailAttachment> emailAttachments = attachments?
                    .Select(attachment =>
                    {
                        return new EmailAttachment()
                        {
                            Content = attachment.AttachmentData,
                            Filename = attachment.FileName
                        };
                    }) ?? [];

                EmailMessage message = new EmailMessage();

                EmailAddress fromAddress = new()
                {
                    DisplayName = "Analytics",
                    Email = "analytics@lylink.org"
                };

                message.From = fromAddress;
                message.To.Add(emailOptions.Value.AnalyticsEmailRecipient!);
                message.Attachments = [.. emailAttachments];
                message.Subject = subject;
                message.HtmlBody = body;

                await resendClient.EmailSendAsync(message);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to send the analytics email.");
            }
        }
    }
}
