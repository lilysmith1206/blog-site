using LylinkBackend_API.Caches;
using LylinkBackend_API.Models;
using LylinkBackend_API.Services;
using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkBackend_DatabaseAccessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace LylinkBackend_API.Controllers
{
    public class PageController(
        IPostDatabaseService postDatabase,
        IPostCategoryDatabaseService categoryDatabase,
        IUserCookieService userCookieService,
        IVisitAnalyticsCache visitAnalyticsCache,
        ISlugCache slugCache) : Controller
    {
        public IActionResult RenderPage(string? editor, string slug = "/")
        {
            IReadOnlyList<string?> postSlugs = slugCache.GetPostSlugs();
            IReadOnlyList<string?> categorySlugs = slugCache.GetCategorySlugs();

            string visitorId = userCookieService.GetVisitorId(Request.Cookies)
                ?? HttpContext.Items["new_visitor_id"] as string
                ?? throw new InvalidOperationException("Visitor Id creation middleware failed.");

            IActionResult view;

            switch (slug)
            {
                case "404":
                case "403":
                    return CreatePostView(slug, null);
                case "/":
                case "":
                    visitAnalyticsCache.QueueVisitAnalyticsForProcessing(slug, "/", visitorId);

                    return CreateIndexView();
            }

            if (postSlugs.Contains(slug))
            {
                view = CreatePostView(slug, editor);
            }
            else if (categorySlugs.Contains(slug))
            {
                view = CreateCategoryView(slug);
            }
            else
            {
                return Redirect("404");
            }

            visitAnalyticsCache.QueueVisitAnalyticsForProcessing(slug, slug, visitorId);

            return view;
        }

        private ViewResult CreatePostView(string slug, string? editorName)
        {
            Post? post = postDatabase.GetPost(slug);

            if (post == null)
            {
                throw new ArgumentOutOfRangeException($"Slug {slug} is not found despite guardrails in caller method.");
            }

            IEnumerable<PostCategory> parents = postDatabase.GetParentCategoriesFromParentId(post.ParentId);

            return base.View(nameof(PostPage), new PostPage()
            {
                Body = post.Body,
                EditorName = editorName,
                Description = post.Description,
                Keywords = post.Keywords,
                PageName = post.Name ?? string.Empty,
                ParentCategories = ModifyCategoriesToPageLinks(parents),
                Title = post.Title,
                DateUpdated = post.DateModified
            });
        }

        private ViewResult CreateCategoryView(string slug)
        {
            PostCategory? postCategory = categoryDatabase.GetCategoryFromSlug(slug);

            if (postCategory == null)
            {
                throw new NullReferenceException($"Invalid post category for slug {slug}");
            }

            IEnumerable<PageLink> posts = FilterPostsForCategory(postDatabase.GetAllPostsWithParentId(postCategory.CategoryId));
            IEnumerable<PageLink> childCategories = GetChildCategoriesForCategoryPage(postCategory);
            IEnumerable<PostCategory> parentCategories = categoryDatabase.GetParentCategoriesFromCategoryId(postCategory?.CategoryId);

            return base.View(nameof(CategoryPage), new CategoryPage()
            {
                Body = postCategory!.Body,
                Description = postCategory!.Description,
                Keywords = postCategory!.Keywords,
                PageName = postCategory!.CategoryName,
                ParentCategories = ModifyCategoriesToPageLinks(parentCategories),
                Posts = posts,
                SubCategories = childCategories,
                Title = postCategory!.Title
            });
        }

        private ViewResult CreateIndexView()
        {
            PostCategory postCategory = categoryDatabase.GetCategoryFromSlug("/") ?? throw new NullReferenceException($"Index not found for some reason?");

            IEnumerable<PageLink> posts = FilterPostsForCategory(postDatabase.GetAllPostsWithParentId(postCategory.CategoryId));
            IEnumerable<PageLink> childCategories = GetChildCategoriesForCategoryPage(postCategory);

            IEnumerable<PageLink> mostRecentPosts = FilterPostsForCategory(postDatabase.GetRecentlyPublishedPosts(10));

            return View(nameof(IndexPage), new IndexPage()
            {
                Body = postCategory.Body,
                Description = postCategory.Description,
                Keywords = postCategory.Keywords,
                MostRecentPosts = mostRecentPosts,
                PageName = postCategory.CategoryName,
                ParentCategories = [new PageLink {
                    Id = "/",
                    Name = "index"
                }],
                Posts = posts,
                SubCategories = childCategories,
                Title = postCategory.Title
            });
        }

        private static IEnumerable<PageLink> FilterPostsForCategory(IEnumerable<Post> posts)
        {
            var postSlugs = posts.Select(post => post.Slug);

            return posts
                .Where(post => Regex.IsMatch(post.Slug, @"\d{3}") == false)
                .Where(post => post.IsDraft == false)
                .Select(post => new PageLink()
                {
                    Name = post?.Name ?? "Unnamed Post",
                    Id = post?.Slug ?? "404",
                });
        }

        private IEnumerable<PageLink> GetChildCategoriesForCategoryPage(PostCategory postCategory)
        {
            return categoryDatabase.GetChildCategoriesOfCategory(postCategory.CategoryId)
                .Where(childCategory => postDatabase.GetAllPostsWithParentId(childCategory.CategoryId).Any() || categoryDatabase.GetChildCategoriesOfCategory(childCategory.CategoryId).Any())
                .Select(post => new PageLink()
                {
                    Name = post?.CategoryName ?? "Unnamed Post Category",
                    Id = post?.Slug ?? "404"
                });
        }

        protected IEnumerable<PageLink> ModifyCategoriesToPageLinks(IEnumerable<PostCategory> categories)
        {
            return categories
                .Reverse()
                .Select(parent =>
            {
                return new PageLink()
                {
                    Id = parent.Slug ?? "404",
                    Name = parent.CategoryName ?? "Parent category not found!",
                };
            });
        }
    }
}
