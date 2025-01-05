using LylinkBackend_API.Caches;
using LylinkBackend_API.Models;
using LylinkBackend_Database.Models;
using LylinkBackend_Database.Services;
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
            Request.Cookies.TryGetValue("accessToken", out string? accessToken);

            var post = postDatabase.GetPost(slug);

            if (post == null)
            {
                return StatusCode(404);
            }

            var postCategory = categoryDatabase.GetCategoryFromId(post?.ParentId ?? string.Empty);

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

        [HttpPost("/savePost")]
        public IActionResult SaveDraft([FromForm] RemotePost remotePost)
        {
            Request.Cookies.TryGetValue("accessToken", out string? accessToken);

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
                ParentId = postDatabase.GetCategoryFromSlug(parentSlug ?? string.Empty)?.CategoryId,
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

                return RedirectToAction("Publisher", "Management", new { accessToken, successfulPostSubmit = true });
            }
            catch (Exception)
            {
                return StatusCode(500, $"Issue adding/updating post {post.Name}");
            }
        }
    }
}
