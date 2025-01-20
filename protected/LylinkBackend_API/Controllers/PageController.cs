using LylinkBackend_API.Caches;
using LylinkBackend_API.Models;
using LylinkBackend_API.Services;
using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace LylinkBackend_API.Controllers
{
    public class PageController(
        IPageRepository pageRepository,
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
            PostPage post = pageRepository.GetPost(slug) ?? throw new ArgumentOutOfRangeException($"Slug {slug} is not found despite guardrails in caller method.");

            return base.View(nameof(PagePost), new PagePost()
            {
                Body = post.Body,
                EditorName = editorName,
                Description = post.Description,
                Keywords = post.Keywords,
                PageName = post.Name ?? string.Empty,
                Parents = post.Parents,
                Title = post.Title,
                DateUpdated = post.DateModified
            });
        }

        private ViewResult CreateCategoryView(string slug)
        {
            CategoryPage category = pageRepository.GetCategory(slug) ?? throw new NullReferenceException($"Invalid post category for slug {slug}");

            return base.View(nameof(PageCategory), new PageCategory()
            {
                Body = category.Body,
                Description = category.Description,
                Keywords = category.Keywords,
                PageName = category.Name,
                ParentCategories = category.ParentCategories,
                Posts = category.Posts,
                SubCategories = category.ChildrenCategories,
                Title = category.Title
            });
        }

        private ViewResult CreateIndexView()
        {
            CategoryPage category = pageRepository.GetCategory("/") ?? throw new NullReferenceException($"Index category not found for some reason?");

            return View(nameof(IndexPage), new IndexPage()
            {
                Body = category.Body,
                Description = category.Description,
                Keywords = category.Keywords,
                MostRecentPosts = pageRepository.GetRecentlyUpdatedPostInfos(10),
                PageName = category.Name,
                ParentCategories = [KeyValuePair.Create("/", "index")],
                Posts = category.Posts,
                SubCategories = category.ChildrenCategories,
                Title = category.Title
            });
        }
    }
}
