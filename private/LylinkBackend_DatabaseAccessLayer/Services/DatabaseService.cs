using LylinkBackend_DatabaseAccessLayer.Models;
using MySqlConnector;
using System.Text.RegularExpressions;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public class DatabaseService(LylinkdbContext context) : IPostDatabaseService, IPostCategoryDatabaseService, IAnnotationDatabaseService, IDatabaseVersionService
    {
        public IEnumerable<string?> GetAllCategorySlugs()
        {
            string?[] postCategories = [.. context.PostCategories.Select(postCategory => postCategory.Slug)];

            return postCategories;
        }

        public IEnumerable<string?> GetAllPostSlugs()
        {
            string?[] postCategories = [.. context.Posts.Select(post => post.Slug)];

            return postCategories;
        }

        public IEnumerable<Post> GetAllPostsWithParentId(int parentId)
        {
            var postCategory = context.PostCategories
                .FirstOrDefault(dbPostCategory => dbPostCategory.CategoryId == parentId);

            if (postCategory == null)
            {
                throw new ArgumentException($"ParentId {parentId} does not match any post category.");
            }

            bool sortByDateCreated = postCategory.UseDateCreatedForSorting ?? true;

            var postsQuery = context.Posts.Where(post => post.ParentId == parentId);

            return (sortByDateCreated
                ? postsQuery.OrderBy(post => post.DateCreated)
                : postsQuery.OrderByDescending(post => post.DateModified))
                .ToList();
        }

        public IEnumerable<Post> GetRecentlyPublishedPosts(int amount)
        {
            return context.Posts
                .OrderByDescending(post => post.DateModified)
                // Filter draft posts
                .Where(post => post.IsDraft == false)
                // Filter out HTTP error pages
                .Where(post => Regex.IsMatch(post.Slug, @"\d{3}") == false)
                .Take(amount)
                .ToList();
        }

        public Post? GetPost(string slug)
        {
            return context.Posts.SingleOrDefault(post => post.Slug == slug);
        }

        public PostCategory? GetCategoryFromSlug(string slug)
        {
            return context.PostCategories.SingleOrDefault(post => post.Slug == slug);
        }

        public PostCategory? GetCategoryFromId(int categoryId)
        {
            return context.PostCategories.SingleOrDefault(postCategory => postCategory.CategoryId == categoryId);
        }

        public IEnumerable<PostCategory> GetParentCategories(int? categoryId)
        {
            var parents = new List<PostCategory>();

            try
            {
                PostCategory? parent = context.PostCategories
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
                    return [context.PostCategories.Single(postCategory => postCategory.Slug == "")];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with getting post categories: {ex.Message}");
            }

            return parents;
        }

        public IEnumerable<PostCategory> GetChildCategoriesOfCategory(int categoryId)
        {
            return context.PostCategories
                .Where(postCategory => postCategory.ParentId == categoryId)
                .ToList();
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
                existingPost.IsDraft = post.IsDraft;

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
                .Where(annotation => slug == annotation.Slug && annotation.EditorName == editorName)
                .ToList();
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

        public IEnumerable<PostCategory> GetAllCategories()
        {
            return context.PostCategories.ToList();
        }

        public bool UpdateCategory(PostCategory category)
        {
            try
            {
                var existingCategory = context.PostCategories.SingleOrDefault(dbCategory => dbCategory.CategoryId == category.CategoryId);

                if (existingCategory == null)
                {
                    Console.WriteLine("Post not found.");
                    return false;
                }

                existingCategory.Title = category.Title;
                existingCategory.ParentId = category.ParentId;
                // existingCategory = category.Name;
                existingCategory.Keywords = category.Keywords;
                existingCategory.Description = category.Description;
                existingCategory.Body = category.Body;
                existingCategory.UseDateCreatedForSorting = category.UseDateCreatedForSorting;
                existingCategory.Slug = category.Slug;

                return context.SaveChanges() == 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating catgory: {ex.Message}");
                return false;
            }
        }

        public bool CreateCategory(PostCategory category)
        {
            context.PostCategories.Add(category);

            return context.SaveChanges() == 1;
        }

        public string? GetDatabaseVersion()
        {
            try
            {
                return context.DatabaseVersions
                .OrderByDescending(databaseVersion => databaseVersion.UpdatedOn)
                .First().Version;
            }
            catch (MySqlException)
            {
                return null;
            }
        }
            
    }
}