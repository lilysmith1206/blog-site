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
                ISlugRepository repository = scope.ServiceProvider.GetRequiredService<ISlugRepository>();

                slugCache.UpdatePostSlugs(repository.GetPostSlugs());
                slugCache.UpdateCategorySlugs(repository.GetCategorySlugs());

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
