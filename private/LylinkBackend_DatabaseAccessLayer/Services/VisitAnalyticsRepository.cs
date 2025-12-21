using LylinkBackend_DatabaseAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public class VisitAnalyticsRepository(IDbContextFactory<LylinkdbContext> contextFactory) : IVisitAnalyticsRepository
    {

        public bool CreateVisitorAnalytic(VisitAnalytic analytic)
        {
            using var context = contextFactory.CreateDbContext();

            context.Add(analytic);

            return context.SaveChanges() == 1;
        }

        public IEnumerable<VisitAnalytic> GetAllVisitorAnalytics()
        {
            using var context = contextFactory.CreateDbContext();

            return context.VisitAnalytics.ToList();
        }

        public bool DropAllVisitorAnalytics()
        {
            using var context = contextFactory.CreateDbContext();

            int visitAnalyticsCount = context.VisitAnalytics.Count();

            context.VisitAnalytics.RemoveRange(context.VisitAnalytics);

            return context.SaveChanges() == visitAnalyticsCount;
        }
    }
}
