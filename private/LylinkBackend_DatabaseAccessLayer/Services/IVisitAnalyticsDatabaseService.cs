using LylinkBackend_DatabaseAccessLayer.Models;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public interface IVisitAnalyticsDatabaseService
    {
        public bool CreateVisitorAnalytic(VisitAnalytic analytic);

        public IEnumerable<VisitAnalytic> GetAllVisitorAnalytics();

        public bool DropAllVisitorAnalytics();
    }
}
