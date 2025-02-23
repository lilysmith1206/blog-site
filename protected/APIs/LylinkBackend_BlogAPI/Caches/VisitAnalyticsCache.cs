using LylinkBackend_DatabaseAccessLayer.Models;

namespace LylinkBackend_API.Caches
{
    public class VisitAnalyticsCache : IVisitAnalyticsCache
    {
        private readonly List<VisitAnalytic> _queuedVisitAnalytics = [];

        private static readonly object _lock = new();

        public void QueueVisitAnalyticsForProcessing(string slugVisited, string slugGiven, string? visitorId)
        {
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            VisitAnalytic visitAnalytic = new()
            {
                SlugGiven = slugGiven,
                SlugVisited = slugVisited,
                VisitedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone),
                VisitorId = visitorId
            };

            lock (_lock)
            {
                _queuedVisitAnalytics.Add(visitAnalytic);
            }
        }

        public IEnumerable<VisitAnalytic> FlushQueuedVisitAnalytics()
        {
            VisitAnalytic[] visitAnalytics = [];

            lock (_lock)
            {
                visitAnalytics = _queuedVisitAnalytics.ToArray();

                _queuedVisitAnalytics.Clear();
            }

            return visitAnalytics;
        }
    }
}
