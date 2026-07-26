using Lylink.Workers.Email.Models;

namespace Lylink.Workers.Email.Services
{
    public interface IEmailService
    {
        public Task<bool> SendEmail(string toAddress, string subject, string body, IEnumerable<Models.EmailAttachment>? attachments = null);
    }
}
