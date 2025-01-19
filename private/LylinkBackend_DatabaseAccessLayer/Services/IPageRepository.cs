using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Models;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public interface IPageRepository
    {
        public PostPage? GetPost(int id);

        public PostPage? GetPost(string slug);

        public IEnumerable<string?> GetAllPostSlugs();

        public IEnumerable<PostPage> GetRecentlyUpdatedPosts(int amount);

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


        public CategoryPage? GetCategory(string slug);

        public CategoryPage? GetCategory(int categoryId);

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
