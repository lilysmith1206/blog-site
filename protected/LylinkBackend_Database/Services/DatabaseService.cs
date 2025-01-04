using LylinkBackend_Database.Models;

namespace LylinkBackend_Database.Services
{
    public class DatabaseService(LylinkdbContext context) : IPostDatabaseService, IPostCategoryDatabaseService, IAnnotationDatabaseService
    {
        public IEnumerable<string?> GetAllCategorySlugs()
        {
            string?[] postCategories = [.. context.PostHierarchies.Select(postCategory => postCategory.Slug)];

            return postCategories;
        }

        public IEnumerable<string?> GetAllPostSlugs()
        {
            string?[] postCategories = [.. context.Posts.Select(post => post.Slug)];

            return postCategories;
        }

        public IEnumerable<Post> GetAllPostsWithParentId(string parentId)
        {
            var postCategory = context.PostHierarchies
                .FirstOrDefault(dbPostCategory => dbPostCategory.CategoryId == parentId);

            if (postCategory == null)
            {
                throw new ArgumentException($"ParentId {parentId} does not match any post category.");
            }

            bool sortByDateCreated = postCategory.UseDateCreatedForSorting ?? true;

            var postsQuery = context.Posts.Where(post => post.ParentId == parentId.ToString());

            return [.. sortByDateCreated
                ? postsQuery.OrderBy(post => post.DateCreated)
                : postsQuery.OrderByDescending(post => post.DateModified)];
        }

        public IEnumerable<Post> GetRecentlyUpdatedPosts(int amount)
        {
            return [.. context.Posts
                .OrderByDescending(post => post.DateModified)
                .Take(amount)];
        }

        public Post? GetPost(string slug)
        {
            return context.Posts.SingleOrDefault(post => post.Slug == slug);
        }

        public PostHierarchy? GetCategoryFromSlug(string slug)
        {
            return context.PostHierarchies.SingleOrDefault(post => post.Slug == slug);
        }

        public PostHierarchy? GetCategoryFromId(string id)
        {
            return context.PostHierarchies.SingleOrDefault(postCategory => postCategory.CategoryId == id);
        }

        public List<PostHierarchy> GetParentCategories(string categoryId)
        {
            var parents = new List<PostHierarchy>();

            try
            {
                PostHierarchy? parent = context.PostHierarchies
                    .SingleOrDefault(postCategory => postCategory.CategoryId == categoryId);

                if (parent != null)
                {
                    parents.Add(parent);

                    if (parent.ParentId != null)
                    {
                        parents.AddRange(GetParentCategories(parent.ParentId));
                    }
                }
                else if (parent == null)
                {
                    return [context.PostHierarchies.Single(postCategory => postCategory.Slug == "")];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with getting post categories: {ex.Message}");
            }

            return parents;
        }

        public List<PostHierarchy> GetChildCategoriesOfCategory(string categoryId)
        {
            return [.. context.PostHierarchies
                .Where(postCategory => postCategory.ParentId == categoryId)];
        }

        public bool CreatePost(Post post)
        {
            try
            {
                context.Posts.Add(post);
                context.SaveChanges();

                Console.WriteLine($"Post {post.Name} inserted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with inserting post: {ex.Message}");
                return false;
            }
        }

        public bool UpdatePost(Post post)
        {
            try
            {
                var existingPost = context.Posts.SingleOrDefault(dbPost => post.Slug == dbPost.Slug);

                if (existingPost == null)
                {
                    Console.WriteLine("Post not found.");
                    return false;
                }

                existingPost.Title = post.Title;
                existingPost.ParentId = post.ParentId;
                existingPost.DateModified = post.DateModified;
                existingPost.Name = post.Name;
                existingPost.Keywords = post.Keywords;
                existingPost.Description = post.Description;
                existingPost.Body = post.Body;

                context.SaveChanges();

                Console.WriteLine("Post updated successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating post: {ex.Message}");
                return false;
            }
        }

        public IEnumerable<Annotation> GetAnnotations(string slug, string editorName)
        {
            return context.Annotations
                .Where(annotation => slug == annotation.Slug && annotation.EditorName == editorName);
        }

        public string? CreateAnnotation(Annotation annotation)
        {
            context.Annotations.Add(annotation);

            context.SaveChanges();

            return annotation.Id;
        }

        public bool UpdateAnnotation(Annotation annotation)
        {
            var currentAnnotation = context.Annotations.Single(dbAnnotation => dbAnnotation.Id == annotation.Id);

            currentAnnotation.AnnotationContent = annotation.AnnotationContent;
            currentAnnotation.EditorName = annotation.EditorName;
            currentAnnotation.Slug = annotation.Slug;

            return context.SaveChanges() == 1;
        }

        public bool DeleteAnnotation(string annotationId)
        {
            Annotation? annotation = context.Annotations.SingleOrDefault(annotation => annotation.Id == annotationId);

            if (annotation == null)
            {
                return false;
            }

            context.Annotations.Remove(annotation);

            return context.SaveChanges() == 1;
        }

        public bool DeleteAnnotation(Annotation annotation)
        {
            context.Annotations.Remove(annotation);

            return context.SaveChanges() == 1;
        }

        public Annotation? GetAnnotation(string id)
        {
            return context.Annotations.SingleOrDefault(annotation => annotation.Id == id);
        }
    }
}