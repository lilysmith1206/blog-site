using LylinkBackend_DatabaseAccessLayer.Models;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public class SlugRepository(LylinkdbContext context) : ISlugRepository
    {
        public IEnumerable<string> GetPostSlugs()
        {
            return context.Posts.Select(post => post.Slug);
        }

        public IEnumerable<string> GetCategorySlugs()
        {
            return context.PostCategories.Select(category => category.Slug);
        }
    }
}
