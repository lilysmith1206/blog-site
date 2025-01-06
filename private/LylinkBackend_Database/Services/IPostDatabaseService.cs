using LylinkBackend_Database.Models;

namespace LylinkBackend_Database.Services
{
    public interface IPostDatabaseService
    {
        public Post? GetPost(string slug);

        public IEnumerable<string?> GetAllPostSlugs();

        public IEnumerable<Post> GetAllPostsWithParentId(string parentId);

        public IEnumerable<Post> GetRecentlyUpdatedPosts(int amount);

        public bool CreatePost(Post post);

        public bool UpdatePost(Post post);
    }
}
