using LylinkBackend_DatabaseAccessLayer.Models;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public class VisitAnalyticsRepository(LylinkdbContext context) : IVisitAnalyticsRepository
    {

        public bool CreateVisitorAnalytic(VisitAnalytic analytic)
        {
            context.Add(analytic);

            return context.SaveChanges() == 1;
        }

        public IEnumerable<VisitAnalytic> GetAllVisitorAnalytics()
        {
            return context.VisitAnalytics.ToList();
        }

        public bool DropAllVisitorAnalytics()
        {
            int visitAnalyticsCount = context.VisitAnalytics.Count();

            context.VisitAnalytics.RemoveRange(context.VisitAnalytics);

            return context.SaveChanges() == visitAnalyticsCount;
        }
    }
}
