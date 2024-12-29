using LylinkBackend_API.Caches;
using LylinkBackend_Database.Services;

namespace LylinkBackend_API.Services
{
    public class SlugDataService(IServiceScopeFactory scopeFactory, ISlugCacheService slugCache) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested == false)
            {
                using var scope = scopeFactory.CreateScope();
                IDatabaseService database = scope.ServiceProvider.GetRequiredService<IDatabaseService>();

                // Update slugs from the database
                slugCache.UpdatePostSlugs(database.GetAllPostSlugs());
                slugCache.UpdateCategorySlugs(database.GetAllCategorySlugs());

                // Wait 10 seconds before updating slugs again
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
