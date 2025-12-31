using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkShared.Models;

namespace LylinkBackend.Repositories.Mappers
{
    public static class PageLinkMapper
    {
        public static PageLink Map(this Post post)
        {
            return new PageLink
            {
                Description = post.SlugNavigation.Description,
                Name = post.SlugNavigation.Name,
                Slug = post.Slug,
            };
        }

        public static PageLink Map(this PostCategory category)
        {
            return new PageLink
            {
                Description = category.SlugNavigation.Description,
                Name = category.SlugNavigation.Name,
                Slug = category.Slug,
            };
        }
    }
}
