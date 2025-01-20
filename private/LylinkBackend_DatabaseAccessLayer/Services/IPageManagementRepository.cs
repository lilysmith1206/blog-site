using LylinkBackend_DatabaseAccessLayer.BusinessModels;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public interface IPageManagementRepository
    {
        public IEnumerable<KeyValuePair<string, string>> GetAllPosts();

        public IEnumerable<KeyValuePair<string, string>> GetAllCategories();

        public PostInfo GetPost(int id);

        public CategoryInfo GetCategory(int id);

        public int CreatePost(
            string slug,
            string title,
            string name,
            string keywords,
            string description,
            string body,
            bool isDraft,
            int? parentId);

        public int UpdatePost(
            string slug,
            string title,
            string name,
            string keywords,
            string description,
            string body,
            bool isDraft,
            int? parentId);

        public int CreateCategory(
            string slug,
            string title,
            string name,
            string keywords,
            string description,
            string body,
            bool isSortingPostsByDateCreated,
            int? parentId);

        public int UpdateCategory(
            string slug,
            string title,
            string name,
            string keywords,
            string description,
            string body,
            bool isSortingPostsByDateCreated,
            int? parentId);
    }
}
