using Lylink.Site.Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Lylink.Shared.Models;
using Lylink.Management.Api.Client;
using ErrorOr;

namespace Lylink.Site.Management.Controllers
{
    [ApiController]
    [Authorize]
    [Route("Management")]
    public class ManagementController : Controller
    {
        private readonly ILogger<ManagementController> _logger;
        private readonly IManagementApiClient _remoteClient;

        public ManagementController(ILogger<ManagementController> logger, IManagementApiClient remoteClient)
        {
            _logger = logger;
            _remoteClient = remoteClient;
        }

        [HttpGet("/")]
        public IActionResult Management()
        {
            return base.View(nameof(Models.Management), new Models.Management());
        }

        [HttpGet("/categorizer")]
        public async Task<IActionResult> Categorizer()
        {
            var categoryReferences = await _remoteClient.GetCategoriesAsync();

            if (categoryReferences.IsError)
            {
                _logger.LogError("An error occurred while fetching category references.");
            }

            return base.View(nameof(Models.Categorizer), new Categorizer()
            {
                Categories = categoryReferences.Value
                    .Where(category => category.Id != 6)
            });
        }

        [HttpGet("/publisher")]
        public async Task<IActionResult> Publisher([FromQuery] bool? successfulPostSubmit)
        {
            var categoriesReferences = await _remoteClient.GetCategoriesAsync();

            if (categoriesReferences.IsError)
            {
                _logger.LogError("An error occurred while fetching category references.");
            }

            var postsByCategory = await _remoteClient.GetPostsByCategoryAsync();

            if (postsByCategory.IsError)
            {
                _logger.LogError("An error occurred while fetching post reference information.");
            }

            return base.View(nameof(Models.Publisher), new Publisher()
            {
                NavigatedFromFormSubmit = successfulPostSubmit == true,
                Categories = categoriesReferences.Value,
                CategoryPosts = postsByCategory.Value,
            });
        }

        [HttpGet("/getPostFromId")]
        public async Task<ActionResult<PublisherPost>> GetSlugPost([FromQuery] int id)
        {
            var post = await _remoteClient.GetPostById(id);

            if (post.IsError)
                return StatusCode(500);

            return Ok(new PublisherPost()
            {
                Body = post.Value.Body,
                Description = post.Value.Description,
                IsDraft = post.Value.IsDraft,
                Keywords = post.Value.Keywords,
                Name = post.Value.Name,
                ParentId = post.Value.ParentId,
                Slug = post.Value.Slug,
                Title = post.Value.Title
            });
        }

        [HttpGet("/getPostCategoryFromId")]
        public async Task<ActionResult<CategorizerCategory>> GetPostCategoryFromSlug([FromQuery] int categoryId)
        {
            var category = await _remoteClient.GetCategoryById(categoryId);

            if (category.IsError)
                return StatusCode(500);

            return Ok(new CategorizerCategory()
            {
                Body = category.Value.Body,
                Description = category.Value.Description,
                Keywords = category.Value.Keywords,
                Name = category.Value.Name,
                ParentId = category.Value.ParentId,
                PostSortingMethod = category.Value.PostSortingMethod,
                Slug = category.Value.Slug,
                Title = category.Value.Title,
            });
        }

        [HttpPost("/savePost")]
        public async Task<ActionResult> SaveDraft([FromForm] PublisherPost publisherPostInfo)
        {
            if (publisherPostInfo.Slug == null)
            {
                return StatusCode(400, "Slug must be specified.");
            }

            try
            {
                PostInfo postInfo = new()
                {
                    Body = publisherPostInfo.Body ?? throw new NullReferenceException($"{nameof(publisherPostInfo.Body)} is null"),
                    Description = publisherPostInfo.Description ?? throw new NullReferenceException($"{nameof(publisherPostInfo.Description)} is null"),
                    Keywords = publisherPostInfo.Keywords ?? throw new NullReferenceException($"{nameof(publisherPostInfo.Keywords)} is null"),
                    Name = publisherPostInfo.Name ?? throw new NullReferenceException($"{nameof(publisherPostInfo.Name)} is null"),
                    Title = publisherPostInfo.Title ?? throw new NullReferenceException($"{nameof(publisherPostInfo.Title)} is null"),
                    IsDraft = publisherPostInfo.IsDraft ?? throw new NullReferenceException($"{nameof(publisherPostInfo.IsDraft)} is null"),
                    ParentId = publisherPostInfo.ParentId ?? throw new NullReferenceException($"{nameof(publisherPostInfo.ParentId)} is null"),
                    Slug = publisherPostInfo.Slug
                };

                var createResult = await _remoteClient.CreatePost(postInfo);

                if (!createResult.IsError)
                    return RedirectToAction("Publisher", "Management", new { successfulPostSubmit = true });
                else if (createResult.FirstError.Type == ErrorType.Conflict)
                {
                    var conflictingId = createResult.FirstError.Metadata!["conflict"];

                    if (conflictingId is not int id)
                        return StatusCode(500);

                    var updateResult = await _remoteClient.UpdatePost(id, postInfo);

                    if (updateResult.IsError)
                        return StatusCode(500);

                    return RedirectToAction("Publisher", "Management", new { successfulPostSubmit = true });
                }
                else
                    return StatusCode(500);
            }
            catch (Exception)
            {
                return StatusCode(500, $"Issue adding/updating post {publisherPostInfo.Name}");
            }
        }

        [HttpPost("/saveCategory")]
        public async Task<IActionResult> SaveCategory([FromForm] CategorizerCategory categorizerCategory)
        {
            if (categorizerCategory.Slug == null)
            {
                return StatusCode(400, "Slug must be specified.");
            }

            try
            {
                CategoryInfo categoryInfo = new()
                {

                    Body = categorizerCategory.Body ?? throw new NullReferenceException($"{nameof(categorizerCategory.Body)} is null"),
                    Description = categorizerCategory.Description ?? throw new NullReferenceException($"{nameof(categorizerCategory.Description)} is null"),
                    Keywords = categorizerCategory.Keywords ?? throw new NullReferenceException($"{nameof(categorizerCategory.Keywords)} is null"),
                    Name = categorizerCategory.Name ?? throw new NullReferenceException($"{nameof(categorizerCategory.Name)} is null"),
                    Title = categorizerCategory.Title ?? throw new NullReferenceException($"{nameof(categorizerCategory.Title)} is null"),
                    PostSortingMethod = categorizerCategory.PostSortingMethod ?? throw new NullReferenceException($"{nameof(categorizerCategory.PostSortingMethod)} is null"),
                    ParentId = categorizerCategory.ParentId ?? throw new NullReferenceException($"{nameof(categorizerCategory.ParentId)} is null"),
                    Slug = categorizerCategory.Slug
                };

                var createResult = await _remoteClient.CreateCategory(categoryInfo);

                if (!createResult.IsError)
                    return RedirectToAction("Categorizer", "Management", new { successfulPostSubmit = true });
                else if (createResult.FirstError.Type == ErrorType.Conflict)
                {
                    var conflictingId = createResult.FirstError.Metadata!["conflict"];

                    if (conflictingId is not int id)
                        return StatusCode(500);

                    var updateResult = await _remoteClient.UpdateCategory(id, categoryInfo);

                    if (updateResult.IsError)
                        return StatusCode(500);

                    return RedirectToAction("Categorizer", "Management", new { successfulPostSubmit = true });
                }
                else
                    return StatusCode(500);
            }
            catch (Exception)
            {
                return StatusCode(500, $"Issue adding/updating post {categorizerCategory.Name}");
            }
        }
    }
}
