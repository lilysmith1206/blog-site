using LylinkBackend_DatabaseAccessLayer.Models;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public interface IPostCategoryDatabaseService
    {
        public IEnumerable<string?> GetAllCategorySlugs();

        public PostHierarchy? GetCategoryFromSlug(string slug);

        public IEnumerable<PostHierarchy> GetAllCategories();

        public PostHierarchy? GetCategoryFromId(int categoryId);

        public List<PostHierarchy> GetParentCategories(int? categoryId);

        public List<PostHierarchy> GetChildCategoriesOfCategory(int categoryId);

        public bool UpdateCategory(PostHierarchy category);

        public bool CreateCategory(PostHierarchy category);
    }
}
