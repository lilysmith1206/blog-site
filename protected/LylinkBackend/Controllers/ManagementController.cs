using LylinkBackend_API.Models;
using LylinkBackend_Database.Models;
using LylinkBackend_Database.Services;
using Microsoft.AspNetCore.Mvc;

namespace LylinkBackend_API.Controllers
{
    [ApiController]
    [Route("Management")]
    public class ManagementController(IDatabaseService database, List<ManagementToken> managementAccessTokens) : Controller
    {
        [HttpGet("/login")]
        public IActionResult Login([FromQuery] string? password)
        {
            bool rightPassword = password == "v1p0v5h7LsqnHhGnC88mPgmE06xfD57bK5xLagPiiRDg3dx3Wh";
            
            if (rightPassword)
            {
                Guid accessToken = Guid.NewGuid();

                managementAccessTokens.Add(new ManagementToken
                {
                    Id = accessToken,
                    TokenExpiration = DateTime.Now.AddDays(1)
                });

                return Redirect($"/management?accessToken={accessToken}");
            }

            return Redirect($"/403");
        }

        [HttpGet("/management")]
        public IActionResult Management([FromQuery] string? accessToken)
        {
            StatusCodeResult? result = VerifyAccessToken(accessToken);

            if (result == null && accessToken is not null)
            {
                return View(nameof(ManagementHome), new ManagementHome()
                {
                    AccessToken = accessToken
                });
            }

            return Redirect("/403");
        }

        [HttpGet("/publisher")]
        public IActionResult Publisher([FromQuery] string? accessToken, [FromQuery] bool? successfulPostSubmit)
        {
            StatusCodeResult? result = VerifyAccessToken(accessToken);

            if (result == null && accessToken is not null)
            {
                return base.View(nameof(Models.Publisher), new Publisher()
                {
                    AccessToken = accessToken,
                    NavigatedFromFormSubmit = successfulPostSubmit == true,
                    AvailableCategories = database.GetAllCategorySlugs(),
                    AvailableSlugs = database.GetAllPostSlugs(),
                });
            }

            return Redirect("/403");
        }

        [HttpPost("/ping")]
        public IActionResult Ping([FromBody] object body)
        {
            return Ok("yeah?");
        }

        [HttpGet("/getSlugPost")]
        public IActionResult GetSlugPost([FromQuery] string accessToken, [FromQuery] string slug)
        {
            StatusCodeResult? tokenVerificationResult = VerifyAccessToken(accessToken);

            if (tokenVerificationResult != null)
            {
                return tokenVerificationResult;
            }

            var post = database.GetPost(slug);

            if (post == null)
            {
                return StatusCode(404);
            }

            var postCategory = database.GetCategoryFromId(post?.ParentId ?? string.Empty);

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

        [HttpPost("/saveDraft")]
        public IActionResult SaveDraft([FromForm] string accessToken, [FromForm] RemotePost remotePost)
        {
            StatusCodeResult? tokenVerificationResult = VerifyAccessToken(accessToken);

            if (tokenVerificationResult != null)
            {
                return tokenVerificationResult;
            }

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

            Post? existingPost = database.GetPost(remotePost.Slug);
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            DateTime currentEasternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

            var post = new Post
            {
                Slug = remotePost.Slug,
                DateModified = currentEasternTime,
                DateCreated = existingPost == null ? currentEasternTime : existingPost.DateCreated,
                Name = remotePost.Name,
                Title = remotePost.Title,
                ParentId = database.GetCategoryFromSlug(parentSlug ?? string.Empty)?.CategoryId,
                Keywords = remotePost.Keywords,
                Description = remotePost.Description,
                Body = remotePost.Body
            };

            try
            {
                if (existingPost != null)
                {
                    database.UpdatePost(post);
                }
                else
                {
                    database.CreatePost(post);
                }

                return RedirectToAction("Publisher", "Management", new { accessToken, successfulPostSubmit = true });
            }
            catch (Exception)
            {
                return StatusCode(500, $"Issue adding/updating post {post.Name}");
            }
        }

        private StatusCodeResult? VerifyAccessToken(string? accessToken)
        {
            ManagementToken? managementToken = managementAccessTokens.SingleOrDefault(token => token.Id.ToString() == accessToken);

            if (managementToken?.Id == null || managementToken?.TokenExpiration < DateTime.Now)
            {
                return StatusCode(403);
            }

            return null;
        }
    }
}
