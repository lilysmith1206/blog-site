﻿using LylinkBackend_API.Caches;
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

            IEnumerable<(string slug, string? name)> posts = database.GetAllPostsWithParentId(postCategory.CategoryId)
                .Select(post => (post.Slug, post.Name));

            IEnumerable<(string? slug, string? name)> childCategories = database.GetChildCategoriesOfCategory(postCategory.CategoryId)
                .Where(childCategory => database.GetAllPostsWithParentId(childCategory.CategoryId).Any()
                    || database.GetChildCategoriesOfCategory(childCategory.CategoryId).Count != 0)
                .Select(post => (post.Slug, post.Name));


            string body = slug == ""
                ? CreateIndexBody(posts, childCategories, postCategory.Body)
                : CreateCategoryBody(posts, childCategories, postCategory.Body);

            return View("PostPage", new PostPage()
            {
                Body = body,
                EditorName = null,
                Description = postCategory.Description ?? string.Empty,
                Keywords = postCategory.Keywords ?? string.Empty,
                PageName = postCategory.Name ?? string.Empty,
                ParentHeader = GetParentCategories(postCategory.ParentId ?? string.Empty),
                Title = postCategory.Title ?? string.Empty,
                DateUpdated = null
            });
        }

        private string CreateIndexBody(IEnumerable<(string slug, string? name)> posts, IEnumerable<(string? slug, string? name)> postCategories, string? categoryBody)
        {
            var mostRecentUpdatedPosts = database.GetRecentlyUpdatedPosts(10)
                .Where(post => Regex.IsMatch(post.Slug, @"\d{3}") == false)
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

        private static string CreateCategoryBody(IEnumerable<(string slug, string? name)> posts, IEnumerable<(string? slug, string? name)> postCategories, string? categoryBody)
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

        protected IEnumerable<ParentHeader> GetParentCategories(string parentId)
        {
            List<PostHierarchy> parents = database.GetParentCategories(parentId);

            parents.Single(parent => string.IsNullOrEmpty(parent.Slug)).Slug = "/"; 

            parents.Reverse();

            return parents.Select(parent =>
            {
                return new ParentHeader()
                {
                    Slug = parent.Slug ?? "404",
                    Name = parent.Name ?? "Parent category not found!",
                };
            });
        }

        protected static bool CheckPostBodyForTableElement(string? postBody)
        {
            Regex tableTagRegex = new(@"<table\b[^>]*>", RegexOptions.IgnoreCase);

            return tableTagRegex.IsMatch(postBody ?? string.Empty);
        }
    }
}