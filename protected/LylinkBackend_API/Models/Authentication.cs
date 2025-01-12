namespace LylinkBackend_API.Models
{
    public class Authentication
    {
        public string SendGridAPIKey { get; set; } = string.Empty;

        public List<string> UserIds { get; set; } = [];
    }
}
