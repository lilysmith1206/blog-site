using LylinkBackend_DatabaseAccessLayer.Models;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public interface IVisitAnalyticsRepository
    {
        public bool CreateVisitorAnalytic(VisitAnalytic analytic);

        public IEnumerable<VisitAnalytic> GetAllVisitorAnalytics();

        public bool DropAllVisitorAnalytics();
    }
}
