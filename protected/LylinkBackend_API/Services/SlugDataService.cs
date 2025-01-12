using LylinkBackend_API.Caches;
using LylinkBackend_DatabaseAccessLayer.Services;

namespace LylinkBackend_API.Services
{
    public class SlugDataService(IServiceScopeFactory scopeFactory, ISlugCache slugCache) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested == false)
            {
                using var scope = scopeFactory.CreateScope();
                IPostDatabaseService postDatabase = scope.ServiceProvider.GetRequiredService<IPostDatabaseService>();
                IPostCategoryDatabaseService categoryDatabase = scope.ServiceProvider.GetRequiredService<IPostCategoryDatabaseService>();

                slugCache.UpdatePostSlugs(postDatabase.GetAllPostSlugs());
                slugCache.UpdateCategorySlugs(categoryDatabase.GetAllCategorySlugs());

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
