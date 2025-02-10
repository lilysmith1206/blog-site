using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Mappers;
using LylinkBackend_DatabaseAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public class PageManagementRepository(LylinkdbContext context) : IPageManagementRepository
    {
        public IEnumerable<CategoryInfo> GetAllCategories()
        {
            IEnumerable<PostCategory> categories = context.PostCategories
                .Include(category => category.SlugNavigation)
                .Include(category => category.PostSortingMethod);

            foreach (PostCategory category in categories)
            {
                BusinessModels.PostSortingMethod sortingMethod = (category.PostSortingMethod?.Map()) ?? throw new NullReferenceException($"Category {category.SlugNavigation.Name} has no sorting method defined.");
                
                category.Map(sortingMethod, out CategoryInfo categoryInfo);
                
                yield return categoryInfo;
            }
        }

        public IEnumerable<PostInfo> GetAllPosts(string? parentSlug = null)
        {
            IEnumerable<Post> posts = context.Posts
                .Include(post => post.SlugNavigation)
                .Where(post => parentSlug == null || (post.Parent != null && post.Parent.Slug == parentSlug))
                .Include(post => post.Parent);

            foreach (Post post in posts)
            {
                post.Map(out PostInfo postInfo);

                yield return postInfo;
            }
        }

        public bool DoesPageWithSlugExist(string slug)
        {
            return context.Pages.Where(page => page.Slug == slug).Any();
        }

        public CategoryInfo GetCategory(int id)
        {
            PostCategory? category = context.PostCategories
                .Where(category => category.CategoryId == id)
                .Include(category => category.SlugNavigation)
                .Include(category => category.PostSortingMethod)
                .SingleOrDefault();

            if (category == null)
            {
                throw new ArgumentOutOfRangeException($"No category with id {id} found.");
            }

            BusinessModels.PostSortingMethod postSortingMethod = category.PostSortingMethod?.Map() ?? throw new NullReferenceException("Post sorting method is null.");

            category.Map(postSortingMethod, out CategoryInfo categoryInfo);

            return categoryInfo;
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

            post.Map(out PostInfo postInfo);

            return postInfo;
        }

        public int CreatePost(PostInfo post)
        {
            Page? existingPage = context.Pages.SingleOrDefault(dbPage => dbPage.Slug == post.Slug);
            Post? existingPost = context.Posts.SingleOrDefault(dbPost => dbPost.Slug == post.Slug);

            if (existingPage is not null || existingPost is not null)
            {
                throw new InvalidOperationException($"Page or post with slug {post.Slug} already exists.");
            }

            Page postPage = new Page()
            {
                Slug = post.Slug,
                Body = post.Body,
                Description = post.Description,
                Keywords = post.Keywords,
                Name = post.Name,
                Title = post.Title
            };

            context.Pages.Add(postPage);

            context.SaveChanges();

            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime currentEasternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

            Post dbPost = new Post()
            {
                Slug = post.Slug,
                DateCreated = currentEasternTime,
                DateModified = currentEasternTime,
                IsDraft = post.IsDraft,
                ParentId = post.ParentId
            };

            context.Posts.Add(dbPost);

            context.SaveChanges();

            return context.Posts.Single(dbPost => dbPost.Slug == post.Slug).Id;
        }

        public int UpdatePost(PostInfo post)
        {
            Page? existingPage = context.Pages.SingleOrDefault(dbPage => dbPage.Slug == post.Slug);
            Post? existingPost = context.Posts.SingleOrDefault(dbPost => dbPost.Slug == post.Slug);

            if (existingPage is null || existingPost is null)
            {
                throw new InvalidOperationException($"Page or post with slug {post.Slug} does not exist.");
            }

            existingPage.Body = post.Body;
            existingPage.Description = post.Description;
            existingPage.Keywords = post.Keywords;
            existingPage.Name = post.Name;
            existingPage.Title = post.Title;

            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime currentEasternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

            existingPost.DateModified = currentEasternTime;
            existingPost.IsDraft = post.IsDraft;
            existingPost.ParentId = post.ParentId;

            context.SaveChanges();

            return context.Posts.Single(dbPost => dbPost.Slug == post.Slug).Id;
        }


        public int CreateCategory(CategoryInfo category)
        {
            Page? existingPage = context.Pages.SingleOrDefault(dbPage => dbPage.Slug == category.Slug);
            PostCategory? existingCategory = context.PostCategories.SingleOrDefault(dbCategory => dbCategory.Slug == category.Slug);

            if (existingPage is not null || existingCategory is not null)
            {
                throw new InvalidOperationException($"Page or post with slug {category.Slug} already exists.");
            }

            Page categoryPage = new Page()
            {
                Slug = category.Slug,
                Body = category.Body,
                Description = category.Description,
                Keywords = category.Keywords,
                Name = category.Name,
                Title = category.Title
            };

            context.Pages.Add(categoryPage);

            context.SaveChanges();

            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime currentEasternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

            PostCategory dbCategory = new()
            {
                Slug = category.Slug,
                ParentId = category.ParentId,
                PostSortingMethodId = (int?)category.PostSortingMethod
            };

            context.PostCategories.Add(dbCategory);

            context.SaveChanges();

            return context.PostCategories.Single(dbCategory => dbCategory.Slug == category.Slug).CategoryId;
        }

        public int UpdateCategory(CategoryInfo category)
        {
            Page? existingPage = context.Pages.SingleOrDefault(page => page.Slug == category.Slug);
            PostCategory? existingCategory = context.PostCategories.SingleOrDefault(dbCategory => dbCategory.Slug == category.Slug);

            if (existingPage is null || existingCategory is null)
            {
                throw new InvalidOperationException($"Page or post with slug {category.Slug} does not exist.");
            }

            existingPage.Body = category.Body;
            existingPage.Description = category.Description;
            existingPage.Keywords = category.Keywords;
            existingPage.Name = category.Name;
            existingPage.Title = category.Title;

            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime currentEasternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

            existingCategory.ParentId = category.ParentId;
            existingCategory.PostSortingMethodId = (int?)category.PostSortingMethod;

            context.SaveChanges();

            return context.PostCategories.Single(dbCategory => dbCategory.Slug == category.Slug).CategoryId;
        }

    }
}
