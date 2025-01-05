using LylinkBackend_API.Caches;
using LylinkBackend_API.Models;
using LylinkBackend_Database.Models;
using LylinkBackend_Database.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace LylinkBackend_API.Controllers
{
    public class PageController(IPostDatabaseService postDatabase, IPostCategoryDatabaseService categoryDatabase, ISlugCache slugCache) : Controller
    {
        public IActionResult RenderPage(string? editor, string slug = "")
        {
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
            Post? post = postDatabase.GetPost(slug);
            
            return View(nameof(PostPage), new PostPage()
            {
                Body = post?.Body ?? string.Empty,
                EditorName = editorName,
                Description = post?.Description ?? string.Empty,
                Keywords = post?.Keywords ?? string.Empty,
                PageName = post?.Name ?? string.Empty,
                ParentCategories = GetParentCategories(post?.ParentId ?? string.Empty),
                Title = post?.Title ?? string.Empty,
                DateUpdated = post?.DateModified ?? DateTime.Now
            });
        }

        private ViewResult CreateCategoryView(string slug)
        {
            PostHierarchy postCategory = postDatabase.GetCategoryFromSlug(slug) ?? throw new NullReferenceException($"Invalid post category for slug {slug}");

            IEnumerable<PageLink> posts = GetPostsUnderCategory(postCategory.CategoryId);
            IEnumerable<PageLink> childCategories = GetChildCategoriesForCategoryPage(postCategory);

            return View(nameof(CategoryPage), new CategoryPage()
            {
                Body = postCategory?.Body ?? "Post category body null",
                Description = postCategory?.Description ?? string.Empty,
                Keywords = postCategory?.Keywords ?? string.Empty,
                PageName = postCategory?.Name ?? string.Empty,
                ParentCategories = GetParentCategories(postCategory?.ParentId ?? string.Empty),
                Posts = posts,
                SubCategories = childCategories,
                Title = postCategory?.Title ?? string.Empty
            });
        }

        private ViewResult CreateIndexView()
        {
            PostHierarchy postCategory = postDatabase.GetCategoryFromSlug("") ?? throw new NullReferenceException($"Index not found for some reason?");

            IEnumerable<PageLink> posts = GetPostsUnderCategory(postCategory.CategoryId);
            IEnumerable<PageLink> childCategories = GetChildCategoriesForCategoryPage(postCategory);

            IEnumerable<PageLink> mostRecentPosts = postDatabase.GetRecentlyUpdatedPosts(10)
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
                ParentCategories = GetParentCategories(postCategory?.ParentId ?? string.Empty),
                Posts = posts,
                SubCategories = childCategories,
                Title = postCategory?.Title ?? string.Empty
            });
        }

        private IEnumerable<PageLink> GetPostsUnderCategory(string categoryId)
        {
            return postDatabase.GetAllPostsWithParentId(categoryId)
                .Where(post => Regex.IsMatch(post.Slug, @"\d{3}") == false)
                .Select(post => new PageLink()
                {
                    Name = post?.Name ?? "Unnamed Post",
                    Slug = post?.Slug ?? "404",
                });
        }

        private IEnumerable<PageLink> GetChildCategoriesForCategoryPage(PostHierarchy postCategory)
        {
            return categoryDatabase.GetChildCategoriesOfCategory(postCategory.CategoryId)
                .Where(childCategory => postDatabase.GetAllPostsWithParentId(childCategory.CategoryId).Any() || categoryDatabase.GetChildCategoriesOfCategory(childCategory.CategoryId).Any())
                .Select(post => new PageLink()
                {
                    Name = post?.Name ?? "Unnamed Post Category",
                    Slug = post?.Slug ?? "404"
                });
        }

        protected IEnumerable<PageLink> GetParentCategories(string parentId)
        {
            List<PostHierarchy> parents = categoryDatabase.GetParentCategories(parentId);

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
