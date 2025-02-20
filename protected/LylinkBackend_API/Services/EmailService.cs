﻿using LylinkBackend_API.Models;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace LylinkBackend_API.Services
{
    public class EmailService(IOptions<Authentication> authentication) : IEmailService
    {
        public async Task SendEmail(string toAddress, string subject, string body, IEnumerable<EmailAttachment>? attachments = null)
        {
            SendGridClient client = new(authentication.Value.SendGridAPIKey);

            EmailAddress fromAddress = new("analytics@lylink.org", "Analytics");

            SendGridMessage message = MailHelper.CreateSingleEmail(fromAddress, new EmailAddress(toAddress), subject, body, body);

            if (attachments != null)
            {
                foreach (EmailAttachment attachmentData in attachments)
                {
                    message.AddAttachment(
                        filename: attachmentData.FileName,
                        base64Content: Convert.ToBase64String(attachmentData.AttachmentData),
                        disposition: "attachment"
                    );
                }
            }

            Response response = await client.SendEmailAsync(message);

            if ((int)response.StatusCode >= 400)
            {
                throw new Exception($"Email sending failed with status: {response.StatusCode}");
            }
        }
    }
}
