using LylinkBackend.Repositories.Mappers;
using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkShared.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace LylinkBackend.Repositories.Management
{
    public class PageManagementRepository(IDbContextFactory<LylinkdbContext> contextFactory) : IPageManagementRepository
    {
        public IEnumerable<CategoryInfo> GetAllCategories()
        {
            try
            {
                using var context = contextFactory.CreateDbContext();

                IEnumerable<PostCategory> categories = context.PostCategories
                    .Include(category => category.SlugNavigation)
                    .Include(category => category.PostSortingMethod)
                    .ToList();

                return categories
                    .Select(category =>
                    {
                        LylinkShared.Models.PostSortingMethod sortingMethod = (category.PostSortingMethod?.Map()) ?? throw new NullReferenceException($"Category {category.SlugNavigation.Name} has no sorting method defined.");

                        category.Map(sortingMethod, out CategoryInfo categoryInfo);

                        return categoryInfo;
                    });
            }
            catch (MySqlException)
            {
                return [];
            }
        }

        public IEnumerable<PostInfo> GetAllPosts(string? parentSlug = null)
        {
            try
            {
                using var context = contextFactory.CreateDbContext();

                IEnumerable<Post> posts = context.Posts
                    .Include(post => post.SlugNavigation)
                    .Where(post => parentSlug == null || (post.Parent != null && post.Parent.Slug == parentSlug))
                    .Include(post => post.Parent)
                    .ToList();

                return posts
                    .Select(post =>
                    {
                        post.Map(out PostInfo postInfo);

                        return postInfo;
                    });
            }
            catch (MySqlException)
            {
                return [];
            }
        }

        public bool DoesPageWithSlugExist(string slug)
        {
            try
            {
                using var context = contextFactory.CreateDbContext();

                return context.Pages.Where(page => page.Slug == slug).Any();
            }
            catch (MySqlException)
            {
                return false;
            }
        }

        public CategoryInfo GetCategory(int id)
        {
            using var context = contextFactory.CreateDbContext();

            PostCategory? category = context.PostCategories
                .Where(category => category.CategoryId == id)
                .Include(category => category.SlugNavigation)
                .Include(category => category.PostSortingMethod)
                .SingleOrDefault();

            if (category == null)
            {
                throw new ArgumentOutOfRangeException($"No category with id {id} found.");
            }

            LylinkShared.Models.PostSortingMethod postSortingMethod = category.PostSortingMethod?.Map() ?? throw new NullReferenceException("Post sorting method is null.");

            category.Map(postSortingMethod, out CategoryInfo categoryInfo);

            return categoryInfo;
        }

        public PostInfo GetPost(int id)
        {
            using var context = contextFactory.CreateDbContext();

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
            using var context = contextFactory.CreateDbContext();

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
            using var context = contextFactory.CreateDbContext();

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
            using var context = contextFactory.CreateDbContext();

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
            using var context = contextFactory.CreateDbContext();

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
