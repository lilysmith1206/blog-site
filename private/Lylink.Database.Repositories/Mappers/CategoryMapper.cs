using Lylink.Database.Context.Models;
using Lylink.Shared.Models;

namespace Lylink.Database.Repositories.Mappers
{
    public static class CategoryMapper
    {
        public static void Map(
            this PostCategory category,
            IEnumerable<PageLink> posts,
            IEnumerable<PageLink> parents,
            IEnumerable<PageLink> childrenCategories,
            Lylink.Shared.Models.PostSortingMethod sortingMethod,
            out CategoryPage page)
        {
            Page databasePage = category.SlugNavigation;

            page = new CategoryPage
            {
                Body = databasePage.Body,
                Description = databasePage.Description,
                Keywords = databasePage.Keywords,
                Name = databasePage.Name,
                ParentCategories = parents,
                ChildrenCategories = childrenCategories,
                Posts = posts,
                Slug = databasePage.Slug,
                Title = databasePage.Title,
                PostSortingMethod = sortingMethod
            };
        }

        public static void Map(this PostCategory category, Lylink.Shared.Models.PostSortingMethod postSortingMethod, out CategoryInfo categoryInfo)
        {
            categoryInfo = new CategoryInfo()
            {
                Id = category.CategoryId,
                Body = category.SlugNavigation.Body,
                Description = category.SlugNavigation.Description,
                Keywords = category.SlugNavigation.Keywords,
                Name = category.SlugNavigation.Name,
                ParentId = category.ParentId,
                Slug = category.SlugNavigation.Slug,
                Title = category.SlugNavigation.Title,
                PostSortingMethod = postSortingMethod
            };
        }
    }
}
