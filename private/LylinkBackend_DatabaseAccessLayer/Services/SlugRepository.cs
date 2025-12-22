using LylinkBackend_DatabaseAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public class SlugRepository(IDbContextFactory<LylinkdbContext> contextFactory) : ISlugRepository
    {
        public IEnumerable<string> GetPostSlugs()
        {
            using var context = contextFactory.CreateDbContext();

            return context.Posts.Select(post => post.Slug).ToList();
        }

        public IEnumerable<string> GetCategorySlugs()
        {
            using var context = contextFactory.CreateDbContext();

            return context.PostCategories.Select(category => category.Slug).ToList();
        }
    }
}
