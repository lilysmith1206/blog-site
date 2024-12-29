using LylinkBackend_API.Caches;
using LylinkBackend_API.Models;
using LylinkBackend_Database.Models;
using LylinkBackend_Database.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.RegularExpressions;

namespace LylinkBackend_API.Controllers
{
    public class PageController(IDatabaseService database, ISlugCacheService slugCache) : Controller
    {
        public IActionResult RenderPage(string? editor, string slug = "")
        {
            bool addSuggestionTools = string.IsNullOrEmpty(editor) == false;

            IReadOnlyList<string?> postSlugs = slugCache.GetPostSlugs();
            IReadOnlyList<string?> categorySlugs = slugCache.GetCategorySlugs();

            if (postSlugs.Contains(slug))
            {
                return CreatePostView(slug, editor);
            }
            else if (slug == "")
            {
                return CreateIndexView();
            }
            else if (categorySlugs.Contains(slug))
            {
                return CreateCategoryView(slug);
            }
            else
            {
                return Redirect("404");
            }
        }

        private ViewResult CreatePostView(string slug, string? editorName)
        {
            Post? post = database.GetPost(slug);
            
            return View(nameof(PostPage), new PostPage()
            {
                Body = post?.Body ?? string.Empty,
                EditorName = editorName,
                Description = post?.Description ?? string.Empty,
                Keywords = post?.Keywords ?? string.Empty,
                PageName = post?.Name ?? string.Empty,
                ParentHeader = GetParentCategories(post?.ParentId ?? string.Empty),
                Title = post?.Title ?? string.Empty,
                DateUpdated = post?.DateModified ?? DateTime.Now
            });
        }

        private ViewResult CreateCategoryView(string slug)
        {
            PostHierarchy postCategory = database.GetCategoryFromSlug(slug) ?? throw new NullReferenceException($"Invalid post category for slug {slug}");

            IEnumerable<PageLink> posts = GetPostsUnderCategory(database, postCategory.CategoryId);
            IEnumerable<PageLink> childCategories = GetChildCategoriesForCategoryPage(database, postCategory);

            return View(nameof(CategoryPage), new CategoryPage()
            {
                Body = postCategory?.Body ?? "Post category body null",
                Description = postCategory?.Description ?? string.Empty,
                Keywords = postCategory?.Keywords ?? string.Empty,
                PageName = postCategory?.Name ?? string.Empty,
                ParentHeader = GetParentCategories(postCategory?.ParentId ?? string.Empty),
                Posts = posts,
                SubCategories = childCategories,
                Title = postCategory?.Title ?? string.Empty
            });
        }

        private ViewResult CreateIndexView()
        {
            PostHierarchy postCategory = database.GetCategoryFromSlug("") ?? throw new NullReferenceException($"Index not found for some reason?");

            IEnumerable<PageLink> posts = GetPostsUnderCategory(database, postCategory.CategoryId);
            IEnumerable<PageLink> childCategories = GetChildCategoriesForCategoryPage(database, postCategory);

            IEnumerable<PageLink> mostRecentPosts = database.GetRecentlyUpdatedPosts(10)
                .Where(post => Regex.IsMatch(post.Slug, @"\d{3}") == false)
                .Select(post => new PageLink()
                {
                    Name = post?.Name ?? "Unnamed Post",
                    Slug = post?.Slug ?? "404",
                });

            return View(nameof(IndexPage), new IndexPage()
            {
                Body = postCategory?.Body ?? "Post category body null",
                Description = postCategory?.Description ?? string.Empty,
                Keywords = postCategory?.Keywords ?? string.Empty,
                MostRecentPosts = mostRecentPosts,
                PageName = postCategory?.Name ?? string.Empty,
                ParentHeader = GetParentCategories(postCategory?.ParentId ?? string.Empty),
                Posts = posts,
                SubCategories = childCategories,
                Title = postCategory?.Title ?? string.Empty
            });
        }

        private static IEnumerable<PageLink> GetPostsUnderCategory(IDatabaseService database, string categoryId)
        {
            return database.GetAllPostsWithParentId(categoryId)
                .Select(post => new PageLink()
                {
                    Name = post?.Name ?? "Unnamed Post",
                    Slug = post?.Slug ?? "404",
                });
        }

        private static IEnumerable<PageLink> GetChildCategoriesForCategoryPage(IDatabaseService database, PostHierarchy postCategory)
        {
            return database.GetChildCategoriesOfCategory(postCategory.CategoryId)
                .Where(childCategory => database.GetAllPostsWithParentId(childCategory.CategoryId).Any() || database.GetChildCategoriesOfCategory(childCategory.CategoryId).Any())
                .Select(post => new PageLink()
                {
                    Name = post?.Name ?? "Unnamed Post Category",
                    Slug = post?.Slug ?? "404"
                });
        }

        protected IEnumerable<PageLink> GetParentCategories(string parentId)
        {
            List<PostHierarchy> parents = database.GetParentCategories(parentId);

            parents.Single(parent => string.IsNullOrEmpty(parent.Slug)).Slug = "/"; 

            parents.Reverse();

            return parents.Select(parent =>
            {
                return new PageLink()
                {
                    Slug = parent.Slug ?? "404",
                    Name = parent.Name ?? "Parent category not found!",
                };
            });
        }
    }
}
