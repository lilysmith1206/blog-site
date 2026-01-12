using Resend;

namespace LylinkBackend_EmailService.Services
{
    public class EmailService(IResend resendClient) : IEmailService
    {
        public async Task<bool> SendEmail(string toAddress, string subject, string body, IEnumerable<Models.EmailAttachment>? attachments = null)
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
                    Email = "daily@analytics.lylink.org"
                };

                message.From = fromAddress;
                message.To.Add(toAddress);
                message.Attachments = [.. emailAttachments];
                message.Subject = subject;
                message.HtmlBody = body;

                await resendClient.EmailSendAsync(message);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send the analytics email: {0}", ex);

                return false;
            }
        }
    }
}
