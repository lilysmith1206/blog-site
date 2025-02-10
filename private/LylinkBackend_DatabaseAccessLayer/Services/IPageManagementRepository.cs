using LylinkBackend_DatabaseAccessLayer.BusinessModels;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public interface IPageManagementRepository
    {
        public IEnumerable<PostInfo> GetAllPosts(string? parentSlug = null);

        public IEnumerable<CategoryInfo> GetAllCategories();

        public PostInfo GetPost(int id);

        public CategoryInfo GetCategory(int id);

        public bool DoesPageWithSlugExist(string slug);

        public int CreatePost(PostInfo post);

        public int UpdatePost(PostInfo post);

        public int CreateCategory(CategoryInfo categoryInfo);

        public int UpdateCategory(CategoryInfo categoryInfo);
    }
}
