using Microsoft.EntityFrameworkCore;
using Lylink.Database.Context.Models;

namespace Lylink.Database.Repositories.Analytics;

public class VisitAnalyticsRepository(IDbContextFactory<LylinkdbContext> contextFactory) : IVisitAnalyticsRepository
{
    public int CreateSuccessVisitAnalytic(string sessionId, string visitedSlug, DateTime visitDate)
    {
        using var context = contextFactory.CreateDbContext();
        var analytic = new VisitAnalytic()
        {
            DateCreated = visitDate,
            SessionId = sessionId,
            VisitedSlug = visitedSlug,
        };

        context.Add(analytic);
        context.SaveChanges();

        return analytic.Id;
    }

    public int CreateFailedVisitAnalytic(string sessionId, string attemptedSlug, string redirectedSlug, DateTime visitDate)
    {
        using var context = contextFactory.CreateDbContext();
        var failedAnalytic = new FailedVisitAnalytic()
        {
            DateCreated = visitDate,
            SessionId = sessionId,
            AttemptedSlug = attemptedSlug,
            RedirectedSlug = redirectedSlug
        };

        context.Add(failedAnalytic);
        context.SaveChanges();

        return failedAnalytic.Id;
    }

    public Models.Analytics GetAllVisitorAnalytics()
    {
        using var context = contextFactory.CreateDbContext();

        var analytics = new Models.Analytics()
        {
            SuccessVisits = context.VisitAnalytics.ToList(),
            FailedVisit = context.FailedVisitAnalytics.ToList(),
        };

        return analytics;
    }

    public bool DropAllVisitorAnalytics()
    {
        using var context = contextFactory.CreateDbContext();

        int visitAnalyticsCount = context.VisitAnalytics.Count();

        context.VisitAnalytics.RemoveRange(context.VisitAnalytics);

        return context.SaveChanges() == visitAnalyticsCount;
    }
}
