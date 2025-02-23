using LylinkBackend_EmailService.Models;

namespace LylinkBackend_EmailService.Services
{
    public interface IEmailService
    {
        public Task SendEmail(string toAddress, string subject, string body, IEnumerable<EmailAttachment>? attachments = null);
    }
}
