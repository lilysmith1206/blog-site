namespace Lylink.Database.Repositories.Analytics;

public interface IVisitAnalyticsRepository
{
    public int CreateSuccessVisitAnalytic(string sessionId, string visitedSlug, DateTime visitedOn);

    public int CreateFailedVisitAnalytic(string sessionId, string visitedSlug, string redirectedSlug, DateTime visitedOn);

    public Models.Analytics GetAllVisitorAnalytics();

    public bool DropAllVisitorAnalytics();
}
