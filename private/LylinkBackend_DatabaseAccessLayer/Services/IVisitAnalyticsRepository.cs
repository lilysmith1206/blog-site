using LylinkBackend_DatabaseAccessLayer.Models;

namespace LylinkBackend_DatabaseAccessLayer.Services;

public interface IVisitAnalyticsRepository
{
    public int CreateSuccessVisitAnalytic(string sessionId, string visitedSlug, DateTime visitedOn);

    public int CreateFailedVisitAnalytic(string sessionId, string visitedSlug, string redirectedSlug, DateTime visitedOn);

    public IEnumerable<VisitAnalytic> GetAllVisitorAnalytics();

    public bool DropAllVisitorAnalytics();
}
