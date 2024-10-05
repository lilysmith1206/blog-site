using LylinkBackend_Database.Models;

namespace LylinkBackend_Database.Services
{
    public interface IDatabaseService
    {
        public Post? GetPost(string slug);

        public PostHierarchy? GetCategoryFromSlug(string slug);

        public PostHierarchy? GetCategoryFromId(string id);

        public List<PostHierarchy> GetParentCategories(string categoryId);

        public List<PostHierarchy> GetChildCategoriesOfCategory(string categoryId);

        public IEnumerable<string?> GetAllCategorySlugs();

        public IEnumerable<string?> GetAllPostSlugs();

        public IEnumerable<Post> GetAllPostsWithParentId(string parentId);

        public IEnumerable<Post> GetRecentlyUpdatedPosts(int amount);

        public Annotation? GetAnnotation(string id);

        public IEnumerable<Annotation> GetAnnotations(string slug, string editorName);
        
        public bool CreatePost(Post post);

        public bool UpdatePost(Post post);

        public string? CreateAnnotation(Annotation annotation);

        public bool UpdateAnnotation(Annotation annotation);

        public bool DeleteAnnotation(string annotationId);

        public bool DeleteAnnotation(Annotation annotation);
    }
}