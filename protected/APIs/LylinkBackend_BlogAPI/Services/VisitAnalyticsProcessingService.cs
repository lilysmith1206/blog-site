using LylinkBackend_API.Caches;
using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkBackend_DatabaseAccessLayer.Services;

namespace LylinkBackend_API.Services
{
    public class VisitAnalyticsProcessingService(IServiceProvider serviceProvider, IVisitAnalyticsCache visitAnalyticsCache) : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested == false)
            {
                using var scope = serviceProvider.CreateScope();

                IVisitAnalyticsRepository visitAnalytics = scope.ServiceProvider.GetRequiredService<IVisitAnalyticsRepository>();

                IEnumerable<VisitAnalytic> analytics = visitAnalyticsCache.FlushQueuedVisitAnalytics();

                foreach (VisitAnalytic analytic in analytics)
                {
                    visitAnalytics.CreateVisitorAnalytic(analytic);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
