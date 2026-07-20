using Lylink.Blog.Shared.Page;
using Lylink.Database.Repositories.Pages;
using Lylink.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lylink.Blog.Api.Controllers
{
    public class PageController(IPageRepository pageRepository) : Controller
    {
        [HttpGet("/pages/index")]
        public ActionResult<Page> GetIndexPageData()
        {
            CategoryPage category = pageRepository.GetCategory("/") ?? throw new NullReferenceException($"Index category not found for some reason?");

            var page = new Page()
            {
                HtmlContent = category.Body,
                Children = category.ChildrenCategories.ToList(),
                Metadata = new()
                {
                    Description = category.Description,
                    Keywords = category.Keywords,
                    Title = category.Title
                },
                Name = category.Name,
                Parents = [new PageLink { Description = "", Name = "index", Slug = "/" }],
            };

            return Ok(page);
        }

        [HttpGet("/pages/by-slug/{slug}")]
        public ActionResult<Page> GetPageBySlug(string slug)
        {
            var post = pageRepository.GetPost(slug);

            if (post is not null)
            {
                var page = MapPostToPage(post.Value);

                return Ok(page);
            }

            var category = pageRepository.GetCategory(slug);

            if (category is not null)
            {
                var page = MapCategoryToPage(category.Value);

                return Ok(page);
            }

            return NotFound("Page not found.");
        }

        private static Page MapPostToPage(PostPage post)
        {
            return new Page()
            {
                Children = [],
                HtmlContent = post.Body,
                Metadata = new()
                {
                    Description = post.Description,
                    Keywords = post.Keywords,
                    Title = post.Title,
                    DateModified = post.DateModified
                },
                Name = post.Name,
                Parents = post.Parents.ToList(),
                Posts = []
            };
        }

        private static Page MapCategoryToPage(CategoryPage category)
        {
            return new Page()
            {
                Children = category.ChildrenCategories.ToList(),
                HtmlContent = category.Body,
                Metadata = new()
                {
                    Description = category.Description,
                    Keywords = category.Keywords,
                    Title = category.Title,
                },
                Name = category.Name,
                Parents = category.ParentCategories.ToList(),
                Posts = category.Posts.ToList()
            };
        }

        [HttpGet("/pages/most-recent")]
        public ActionResult<List<PageLink>> GetIndexPageData([FromQuery] int limit = 10)
        {
            var posts = pageRepository.GetRecentlyUpdatedPostInfos(limit);

            return Ok(posts);
        }
    }
}
