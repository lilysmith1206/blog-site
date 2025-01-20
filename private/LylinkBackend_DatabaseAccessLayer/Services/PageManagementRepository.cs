using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public class PageManagementRepository(LylinkdbContext context) : IPageManagementRepository
    {
        public IEnumerable<KeyValuePair<string, string>> GetAllCategories()
        {
            return context.PostCategories
                .Include(category => category.SlugNavigation)
                .Select(category => KeyValuePair.Create(category.CategoryId.ToString(), category.SlugNavigation.Name));
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllPosts()
        {
            return context.Posts
                .Include(post => post.SlugNavigation)
                .Select(post => KeyValuePair.Create(post.Id.ToString(), post.SlugNavigation.Name));
        }

        public CategoryInfo GetCategory(int id)
        {
            PostCategory? category = context.PostCategories
                .Where(category => category.CategoryId == id)
                .Include(category => category.SlugNavigation)
                .SingleOrDefault();

            if (category == null)
            {
                throw new ArgumentOutOfRangeException($"No category with id {id} found.");
            }

            return new CategoryInfo
            {
                Id = id,
                Body = category.SlugNavigation.Body,
                Description = category.SlugNavigation.Description,
                Keywords = category.SlugNavigation.Keywords,
                Name = category.SlugNavigation.Name,
                ParentId = category.ParentId,
                Slug = category.SlugNavigation.Slug,
                Title = category.SlugNavigation.Title,
                UseDateCreatedForSorting = category.UseDateCreatedForSorting
            };
        }

        public PostInfo GetPost(int id)
        {
            Post? post = context.Posts
                .Where(post => post.Id == id)
                .Include(post => post.SlugNavigation)
                .SingleOrDefault();

            if (post == null)
            {
                throw new ArgumentOutOfRangeException($"No post with id {id} found.");
            }

            return new PostInfo
            {
                Id = id,
                Body = post.SlugNavigation.Body,
                Description = post.SlugNavigation.Description,
                Keywords = post.SlugNavigation.Keywords,
                Name = post.SlugNavigation.Name,
                ParentId = post.ParentId,
                Slug = post.SlugNavigation.Slug,
                Title = post.SlugNavigation.Title,
                IsDraft = post.IsDraft
            };
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

    }
}
