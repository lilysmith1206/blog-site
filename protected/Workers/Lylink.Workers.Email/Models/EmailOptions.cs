namespace Lylink.Workers.Email.Models
{
    public class EmailOptions
    {
        public string? AnalyticsEmailRecipient { get; set; }

        public TimeOnly? EasternTimeSent { get; set; }

        public string? ApiKey { get; set; }
    }
}
