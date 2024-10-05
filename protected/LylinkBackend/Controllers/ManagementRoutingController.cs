using LylinkBackend_API.Models;
using LylinkBackend_Database.Models;
using LylinkBackend_Database.Services;
using Microsoft.AspNetCore.Mvc;

namespace LylinkBackend_API.Controllers
{
    [ApiController]
    public class ManagementRoutingController(IDatabaseService database, List<ManagementToken> managementAccessTokens) : ControllerBase
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

            if (result == null)
            {
                string pageHtml = System.IO.File.ReadAllText("./wwwroot/html/management.html")
                    .Replace("ACCESSTOKENREPLACEHERE", accessToken);

                return base.Content(pageHtml, "text/html");
            }

            return Redirect("/403");
        }

        [HttpGet("/publisher")]
        public IActionResult Publisher([FromQuery] string? accessToken)
        {
            StatusCodeResult? result = VerifyAccessToken(accessToken);

            if (result == null)
            {
                string pageHtml = System.IO.File.ReadAllText("./wwwroot/html/publisher.html")
                    .Replace("ACCESSTOKENREPLACEHERE", accessToken);

                return Content(pageHtml, "text/html");
            }

            return Redirect("/403");
        }

        [HttpPost("/ping")]
        public IActionResult Ping([FromBody] object body)
        {
            return Ok("yeah?");
        }

        [HttpGet("/getAllSlugs")]
        public IActionResult GetAllSlugs([FromQuery] string accessToken)
        {
            StatusCodeResult? tokenVerificationResult = VerifyAccessToken(accessToken);

            if (tokenVerificationResult != null)
            {
                return tokenVerificationResult;
            }

            IEnumerable<string?> slugs = database.GetAllPostSlugs();

            return Ok(slugs);
        }

        [HttpGet("/getAllCategoryNames")]
        public IActionResult GetAllCategoryNames([FromQuery] string accessToken)
        {
            StatusCodeResult? tokenVerificationResult = VerifyAccessToken(accessToken);

            if (tokenVerificationResult != null)
            {
                return tokenVerificationResult;
            }

            IEnumerable<string?> categorySlugs = database.GetAllCategorySlugs();
            
            return Ok(categorySlugs);
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
                DateModifiedISO = post?.DateModified?.ToString("o"),
                DateCreatedISO = post?.DateCreated?.ToString("o"),
                ParentSlug = postCategory?.Slug,
                Name = post?.Name,
                Keywords = post?.Keywords,
                Description = post?.Description,
                Body = post?.Body
            };

            return Ok(remotePost);
        }

        [HttpPost("/sendPost")]
        public IActionResult SendPost([FromQuery] string accessToken, [FromBody] RemotePost remotePost)
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

            var post = new Post
            {
                Slug = remotePost.Slug,
                DateModified = DateTime.Parse(remotePost.DateModifiedISO ?? DateTime.UtcNow.ToString()),
                DateCreated = DateTime.Parse(remotePost.DateCreatedISO ?? DateTime.UtcNow.ToString()),
                Name = remotePost.Name,
                Title = remotePost.Title,
                ParentId = database.GetCategoryFromSlug(parentSlug ?? string.Empty)?.CategoryId,
                Keywords = remotePost.Keywords,
                Description = remotePost.Description,
                Body = remotePost.Body
            };

            try
            {
                var existingPost = database.GetPost(post.Slug);

                if (existingPost != null)
                {
                    database.UpdatePost(post);
                }
                else
                {
                    database.CreatePost(post);
                }

                return Ok($"Successful addition/update of post {post.Name}");
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
