using LylinkBackend_DatabaseAccessLayer.Models;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public interface IPostDatabaseService
    {
        public Post? GetPost(string slug);

        public IEnumerable<string?> GetAllPostSlugs();

        public IEnumerable<Post> GetAllPostsWithParentId(int parentId);

        public IEnumerable<Post> GetRecentlyPublishedPosts(int amount);

        public bool CreatePost(Post post);

        public bool UpdatePost(Post post);
    }
}
