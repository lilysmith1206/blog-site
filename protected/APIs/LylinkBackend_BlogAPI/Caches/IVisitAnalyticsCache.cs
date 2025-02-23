using LylinkBackend_DatabaseAccessLayer.Models;

namespace LylinkBackend_API.Caches
{
    public interface IVisitAnalyticsCache
    {
        public void QueueVisitAnalyticsForProcessing(string slugVisited, string slugGiven, string? visitorId);

        public IEnumerable<VisitAnalytic> FlushQueuedVisitAnalytics();
    }
}
