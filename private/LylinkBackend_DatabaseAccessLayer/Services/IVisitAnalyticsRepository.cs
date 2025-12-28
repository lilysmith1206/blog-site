using LylinkBackend_DatabaseAccessLayer.Models;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public interface IVisitAnalyticsRepository
    {
        public int CreateVisitorAnalytic(VisitAnalytic analytic);

        public IEnumerable<VisitAnalytic> GetAllVisitorAnalytics();

        public bool DropAllVisitorAnalytics();
    }
}
