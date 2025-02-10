using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Models;

namespace LylinkBackend_DatabaseAccessLayer.Mappers
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
