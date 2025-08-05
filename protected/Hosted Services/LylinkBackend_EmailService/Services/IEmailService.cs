using LylinkBackend_EmailService.Models;

namespace LylinkBackend_EmailService.Services
{
    public interface IEmailService
    {
        public Task<bool> SendEmail(string toAddress, string subject, string body, IEnumerable<Models.EmailAttachment>? attachments = null);
    }
}
