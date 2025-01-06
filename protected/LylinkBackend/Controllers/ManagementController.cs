using LylinkBackend_API.Models;
using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkBackend_DatabaseAccessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace LylinkBackend_API.Controllers
{
    [ApiController]
    [Route("Management")]
    public class ManagementController(IPostDatabaseService postDatabase, IPostCategoryDatabaseService categoryDatabase) : Controller
    {
        [HttpGet("/management")]
        public IActionResult Management()
        {
            return base.View(nameof(Models.Management), new Management() { });
        }

        [HttpGet("/categorizer")]
        public IActionResult Categorizer()
        {
            return base.View(nameof(Models.Categorizer), new Categorizer()
            {
                CategoryLinks = categoryDatabase.GetAllCategories()
                    .Where(category => category.CategoryId != 6)
                    .Select(category => new PageLink { Id = category.CategoryId.ToString(), Name = category.CategoryName ?? string.Empty })
            });
        }

        [HttpGet("/publisher")]
        public IActionResult Publisher([FromQuery] bool? successfulPostSubmit)
        {
            return base.View(nameof(Models.Publisher), new Publisher()
            {
                NavigatedFromFormSubmit = successfulPostSubmit == true,
                AvailableCategories = categoryDatabase.GetAllCategorySlugs(),
                AvailableSlugs = postDatabase.GetAllPostSlugs(),
            });
        }

        [HttpPost("/ping")]
        public IActionResult Ping([FromBody] object body)
        {
            return Ok("yeah?");
        }

        [HttpGet("/getPostFromSlug")]
        public IActionResult GetSlugPost([FromQuery] string slug)
        {
            var post = postDatabase.GetPost(slug);

            if (post == null)
            {
                return StatusCode(404);
            }

            var postCategory = categoryDatabase.GetCategoryFromId(post.ParentId ?? - 1);

            var remotePost = new RemotePost
            {
                Slug = post?.Slug,
                Title = post?.Title,
                ParentSlug = postCategory?.Slug,
                Name = post?.Name,
                Keywords = post?.Keywords,
                Description = post?.Description,
                Body = post?.Body
            };

            return Ok(remotePost);
        }

        [HttpGet("/getPostCategoryFromId")]
        public IActionResult GetPostCategoryFromSlug([FromQuery] int categoryId)
        {
            var category = categoryDatabase.GetCategoryFromId(categoryId);

            if (category == null)
            {
                return StatusCode(404);
            }

            var postCategory = categoryDatabase.GetCategoryFromId(category.CategoryId);

            var remotePost = new RemoteCategory
            {
                Slug = category?.Slug,
                Title = category?.Title,
                ParentId = postCategory?.ParentId,
                CategoryName = category?.CategoryName,
                CategoryId = category?.CategoryId,
                Keywords = category?.Keywords,
                Description = category?.Description,
                Body = category?.Body,
                UseDateCreatedForSorting = category?.UseDateCreatedForSorting
            };

            return Ok(remotePost);
        }

        [HttpPost("/savePost")]
        public IActionResult SaveDraft([FromForm] RemotePost remotePost)
        {
            if (remotePost.Slug == null)
            {
                return StatusCode(406);
            }

            var parentSlug = remotePost.ParentSlug switch
            {
                "none" => null,
                "" => "",
                _ => remotePost.ParentSlug
            };

            Post? existingPost = postDatabase.GetPost(remotePost.Slug);
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            DateTime currentEasternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

            var post = new Post
            {
                Slug = remotePost.Slug,
                DateModified = currentEasternTime,
                DateCreated = existingPost == null ? currentEasternTime : existingPost.DateCreated,
                Name = remotePost.Name,
                Title = remotePost.Title,
                ParentId = categoryDatabase.GetCategoryFromSlug(parentSlug ?? string.Empty)?.CategoryId,
                Keywords = remotePost.Keywords,
                Description = remotePost.Description,
                Body = remotePost.Body
            };

            try
            {
                if (existingPost != null)
                {
                    postDatabase.UpdatePost(post);
                }
                else
                {
                    postDatabase.CreatePost(post);
                }

                return RedirectToAction("Publisher", "Management", new { successfulPostSubmit = true });
            }
            catch (Exception)
            {
                return StatusCode(500, $"Issue adding/updating post {post.Name}");
            }
        }

        [HttpPost("/saveCategory")]
        public IActionResult SaveCategory([FromForm] RemoteCategory remoteCategory)
        {
            if (remoteCategory.Slug == null || remoteCategory.CategoryId == null)
            {
                return StatusCode(406);
            }

            PostCategory? existingCategory = categoryDatabase.GetCategoryFromId(remoteCategory.CategoryId.Value);

            var category = new PostCategory
            {
                Slug = remoteCategory.Slug,
                CategoryName = remoteCategory.CategoryName,
                Title = remoteCategory.Title,
                ParentId = remoteCategory.ParentId,
                Keywords = remoteCategory.Keywords,
                Description = remoteCategory.Description,
                Body = remoteCategory.Body,
                UseDateCreatedForSorting = remoteCategory.UseDateCreatedForSorting,
            };

            try
            {
                if (existingCategory != null)
                {
                    category.CategoryId = existingCategory.CategoryId;

                    categoryDatabase.UpdateCategory(category);
                }
                else
                {
                    categoryDatabase.CreateCategory(category);
                }

                return RedirectToAction("Categorizer", "Management", new { successfulPostSubmit = true });
            }
            catch (Exception)
            {
                return StatusCode(500, $"Issue adding/updating post {category.CategoryName}");
            }
        }
    }
}
