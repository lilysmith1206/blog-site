using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkShared.Models;

namespace LylinkBackend_DatabaseAccessLayer.Mappers
{
    public static class CategoryMapper
    {
        public static void Map(
            this PostCategory category,
            IEnumerable<PageLink> posts,
            IEnumerable<PageLink> parents,
            IEnumerable<PageLink> childrenCategories,
            LylinkShared.Models.PostSortingMethod sortingMethod,
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

        public static void Map(this PostCategory category, LylinkShared.Models.PostSortingMethod postSortingMethod, out CategoryInfo categoryInfo)
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
