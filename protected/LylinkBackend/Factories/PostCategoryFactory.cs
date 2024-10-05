using LylinkBackend_API.Models;
using LylinkBackend_API.Renderers;
using LylinkBackend_Database.Models;
using LylinkBackend_Database.Services;
using System.Text;

namespace LylinkBackend_API.Factories
{
    public class PostCategoryFactory(IDatabaseService databaseService, IRazorViewToStringRenderer renderer) : PageGenerationHelper(databaseService, renderer)
    {
        public async Task<string> GenerateCategoryPage(string slug)
        {
            PostHierarchy postCategory = databaseService.GetCategoryFromSlug(slug) ?? throw new NullReferenceException($"Invalid post category for slug {slug}");

            IEnumerable<(string slug, string? name)> posts = databaseService.GetAllPostsWithParentId(postCategory.CategoryId)
                .Select(post => (post.Slug, post.Name));

            IEnumerable<(string? slug, string? name)> childCategories = databaseService.GetChildCategoriesOfCategory(postCategory.CategoryId)
                .Where(childCategory => databaseService.GetAllPostsWithParentId(childCategory.CategoryId).Any()
                    || databaseService.GetChildCategoriesOfCategory(childCategory.CategoryId).Count != 0)
                .Select(post => (post.Slug, post.Name));


            string body = slug == ""
                ? CreateIndexBody(posts, childCategories, postCategory.Body)
                : CreateCategoryBody(posts, childCategories, postCategory.Body);

            BasePageModel model = new BasePageModel()
            {
                AddSuggestionTools = false,
                Body = body,
                Description = postCategory.Description ?? string.Empty,
                DoesPostContainTableElement = CheckPostBodyForTableElement(body),
                Keywords = postCategory.Keywords ?? string.Empty,
                PageName = postCategory.Name ?? string.Empty,
                ParentHeader = BuildParentHeader(postCategory.ParentId ?? string.Empty),
                Title = postCategory.Title ?? string.Empty,
                UpdatedDateTime = string.Empty
            };

            return await GeneratePage(model);
        }

        private string CreateCategoryBody(IEnumerable<(string slug, string? name)> posts, IEnumerable<(string? slug, string? name)> postCategories, string? categoryBody)
        {
            var sb = new StringBuilder();
            sb.AppendLine(categoryBody);

            if (postCategories.Any())
            {
                sb.AppendLine("<h3>Sub-categories</h3>");
                sb.AppendLine(CreateListOfPosts(postCategories));
            }

            if (posts.Any())
            {
                sb.AppendLine("<h3>Posts</h3>");
                sb.AppendLine(CreateListOfPosts(posts));
            }

            return sb.ToString();
        }

        private string CreateIndexBody(IEnumerable<(string slug, string? name)> posts, IEnumerable<(string? slug, string? name)> postCategories, string? categoryBody)
        {
            var mostRecentUpdatedPosts = databaseService.GetRecentlyUpdatedPosts(10)
                .Where(post => System.Text.RegularExpressions.Regex.IsMatch(post.Slug, @"\d{3}") == false)
                .Select(post => (post.Slug, post.Name));

            var sb = new StringBuilder();
            sb.AppendLine(CreateCategoryBody(posts, postCategories, categoryBody));
            sb.AppendLine("<h3>Most recently updated posts</h3>");
            sb.AppendLine(CreateListOfPosts(mostRecentUpdatedPosts));

            return sb.ToString();
        }
        
        private static string CreateListOfPosts(IEnumerable<(string Slug, string? Name)> posts)
        {
            if (posts == null || !posts.Any())
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            sb.AppendLine("<ul>");

            foreach (var post in posts)
            {
                sb.AppendLine($@"<li><a href=""{post.Slug}"">{post.Name}</a></li>");
            }

            sb.AppendLine("</ul>");
            return sb.ToString();
        }
    }
}
