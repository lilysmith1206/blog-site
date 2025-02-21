using LylinkBackend_API.Models;

namespace LylinkBackend_API.Services
{
    public interface IEmailService
    {
        public Task SendEmail(string toAddress, string subject, string body, IEnumerable<EmailAttachment>? attachments = null);
    }
}
