using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public class PageRepository(LylinkdbContext context) : IPageRepository
    {
        private readonly PostCategory IndexCategory = context.PostCategories.Single(category => category.Slug == "/");
        
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

        public IEnumerable<PostPage> GetRecentlyUpdatedPosts(int amount)
        {
            return context.Posts
                .OrderByDescending(post => post.DateModified)
                .Where(post => post.IsDraft == false)
                .Take(amount)
                .Include(post => post.SlugNavigation)
                .Select(post => ConvertPostToPostPage(post));
        }

        public PostPage? GetPost(int id)
        {
            Post? databasePost = context.Posts
                .Where(post => post.Id == id)
                .Include(post => post.SlugNavigation)
                .SingleOrDefault();

            if (databasePost == null)
            {
                return null;
            }

            return ConvertPostToPostPage(databasePost);
        }

        public PostPage? GetPost(string slug)
        {
            Post? databasePost = context.Posts
                .Where(post => post.Slug == slug)
                .Include(post => post.SlugNavigation)
                .SingleOrDefault();

            if (databasePost == null)
            {
                return null;
            }

            return ConvertPostToPostPage(databasePost);
        }

        public PostPage ConvertPostToPostPage(Post post)
        {
            List<KeyValuePair<string, string>> parents = [];
            PostCategory? currentParent = post.Parent;

            while (currentParent is not null)
            {
                parents.Add(KeyValuePair.Create(currentParent.Slug, currentParent.SlugNavigation.Name));

                currentParent = currentParent.Parent;
            }

            Page databasePage = post.SlugNavigation;

            PostPage postPage = new PostPage
            {
                Body = databasePage.Body,
                DateCreated = post.DateCreated,
                DateModified = post.DateModified,
                Description = databasePage.Description,
                IsDraft = post.IsDraft,
                Keywords = databasePage.Keywords,
                Name = databasePage.Name,
                Parents = parents,
                Slug = databasePage.Slug,
                Title = databasePage.Title,
            };

            return postPage;
        }

        public CategoryPage? GetCategory(int id)
        {
            PostCategory? databaseCategory = context.PostCategories
                .Where(category => category.CategoryId == id)
                .Include(category => category.InverseParent)
                .Include(category => category.Parent)
                .Include(category => category.SlugNavigation)
                .SingleOrDefault();

            if (databaseCategory == null)
            {
                return null;
            }

            return ConvertCategoryToCategoryPage(databaseCategory);
        }

        public CategoryPage? GetCategory(string slug)
        {
            PostCategory? databaseCategory = context.PostCategories
                .Where(category => category.Slug == slug)
                .Include(category => category.InverseParent)
                .Include(category => category.Parent)
                .Include(category => category.SlugNavigation)
                .SingleOrDefault();

            if (databaseCategory == null)
            {
                return null;
            }

            Page? databasePage = context.Pages.SingleOrDefault(page => page.Slug == databaseCategory.Slug);

            if (databasePage == null)
            {
                return null;
            }

            return ConvertCategoryToCategoryPage(databaseCategory);
        }

        private static CategoryPage ConvertCategoryToCategoryPage(PostCategory databaseCategory)
        {
            List<KeyValuePair<string, string>> categoryParents = [];
            PostCategory? currentParent = databaseCategory.Parent;

            while (currentParent is not null)
            {
                categoryParents.Add(KeyValuePair.Create(currentParent.Slug, currentParent.SlugNavigation.Name));

                currentParent = currentParent.Parent;
            }

            Page databasePage = databaseCategory.SlugNavigation;
            
            CategoryPage categoryPage = new CategoryPage
            {
                Body = databasePage.Body,
                Description = databasePage.Description,
                Keywords = databasePage.Keywords,
                Name = databasePage.Name,
                ParentCategories = categoryParents,
                ChildrenCategories = databaseCategory.InverseParent.Select(category => KeyValuePair.Create(category.Slug, category.SlugNavigation.Name)),
                Posts = databaseCategory.Posts.Where(post => post.IsDraft == false).Select(post => KeyValuePair.Create(post.Slug, post.SlugNavigation.Name)),
                Slug = databasePage.Slug,
                Title = databasePage.Title,
                UseDateCreatedForSorting = databaseCategory.UseDateCreatedForSorting
            };

            return categoryPage;
        }

        public int CreatePost(string slug, string title, string name, string keywords, string description, string body, bool isDraft, int? parentId)
        {
            Page? existingPage = context.Pages.SingleOrDefault(page => page.Slug == slug);
            Post? existingPost = context.Posts.SingleOrDefault(post => post.Slug == slug);

            if (existingPage is not null || existingPost is not null)
            {
                throw new InvalidOperationException($"Page or post with slug {slug} already exists.");
            }

            Page postPage = new Page()
            {
                Slug = slug,
                Body = body,
                Description = description,
                Keywords = keywords,
                Name = name,
                Title = title
            };

            context.Pages.Add(postPage);

            context.SaveChanges();

            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime currentEasternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

            Post post = new Post()
            {
                Slug = slug,
                DateCreated = currentEasternTime,
                DateModified = currentEasternTime,
                IsDraft = isDraft,
                ParentId = parentId
            };

            context.Posts.Add(post);

            context.SaveChanges();

            return context.Posts.Single(post => post.Slug == slug).Id;
        }

        public int UpdatePost(string slug, string title, string name, string keywords, string description, string body, bool isDraft, int? parentId)
        {
            Page? existingPage = context.Pages.SingleOrDefault(page => page.Slug == slug);
            Post? existingPost = context.Posts.SingleOrDefault(post => post.Slug == slug);

            if (existingPage is null || existingPost is null)
            {
                throw new InvalidOperationException($"Page or post with slug {slug} does not exist.");
            }

            existingPage.Body = body;
            existingPage.Description = description;
            existingPage.Keywords = keywords;
            existingPage.Name = name;
            existingPage.Title = title;

            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime currentEasternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

            existingPost.DateModified = currentEasternTime;
            existingPost.IsDraft = isDraft;
            existingPost.ParentId = parentId;

            context.SaveChanges();

            return context.Posts.Single(post => post.Slug == slug).Id;
        }


        public int CreateCategory(string slug, string title, string name, string keywords, string description, string body, bool isSortingPostsByDateCreated, int? parentId)
        {
            Page? existingPage = context.Pages.SingleOrDefault(page => page.Slug == slug);
            PostCategory? existingCategory = context.PostCategories.SingleOrDefault(category => category.Slug == slug);

            if (existingPage is not null || existingCategory is not null)
            {
                throw new InvalidOperationException($"Page or post with slug {slug} already exists.");
            }

            Page categoryPage = new Page()
            {
                Slug = slug,
                Body = body,
                Description = description,
                Keywords = keywords,
                Name = name,
                Title = title
            };

            context.Pages.Add(categoryPage);

            context.SaveChanges();

            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime currentEasternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

            PostCategory category = new()
            {
                Slug = slug,
                ParentId = parentId,
                UseDateCreatedForSorting = isSortingPostsByDateCreated
            };

            context.PostCategories.Add(category);

            context.SaveChanges();

            return context.PostCategories.Single(category => category.Slug == slug).CategoryId;
        }

        public int UpdateCategory(string slug, string title, string name, string keywords, string description, string body, bool isSortingPostsByDateCreated, int? parentId)
        {
            Page? existingPage = context.Pages.SingleOrDefault(page => page.Slug == slug);
            PostCategory? existingCategory = context.PostCategories.SingleOrDefault(category => category.Slug == slug);

            if (existingPage is null || existingCategory is null)
            {
                throw new InvalidOperationException($"Page or post with slug {slug} does not exist.");
            }

            existingPage.Body = body;
            existingPage.Description = description;
            existingPage.Keywords = keywords;
            existingPage.Name = name;
            existingPage.Title = title;

            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime currentEasternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

            existingCategory.ParentId = parentId;
            existingCategory.UseDateCreatedForSorting = isSortingPostsByDateCreated;

            context.SaveChanges();

            return context.PostCategories.Single(category => category.Slug == slug).CategoryId;
        }

        /*

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
        */
    }
}