using LylinkBackend_Database.Models;

namespace LylinkBackend_Database.Services
{
    public interface IPostCategoryDatabaseService
    {
        public IEnumerable<string?> GetAllCategorySlugs();

        public PostHierarchy? GetCategoryFromId(string id);

        public List<PostHierarchy> GetParentCategories(string categoryId);

        public List<PostHierarchy> GetChildCategoriesOfCategory(string categoryId);
    }
}
