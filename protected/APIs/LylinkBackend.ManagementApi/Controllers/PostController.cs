using LylinkBackend.ManagementShared;
using LylinkBackend_DatabaseAccessLayer.Services;
using LylinkShared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LylinkBackend.ManagementApi.Controllers;

[ApiController]
[Authorize]
[Route("/[controller]")]
public class PostController : Controller
{
    private readonly ILogger<CategoryController> _logger;
    private readonly IPageManagementRepository _pageManagementRepository;

    public PostController(ILogger<CategoryController> logger, IPageManagementRepository pageManagementRepository)
    {
        _logger = logger;
        _pageManagementRepository = pageManagementRepository;
    }

    [HttpGet("/posts")]
    public ActionResult<IEnumerable<ReferenceById>> GetPosts()
    {
        IEnumerable<ReferenceById> categories = _pageManagementRepository.GetAllCategories()
            .Select(category => new ReferenceById()
            {
                Id = category.Id,
                Name = category.Name
            });

        return Ok(categories);
    }


    [HttpGet("/posts/by-category")]
    public Dictionary<string, List<ReferenceById>> GetPostsByCategoryName()
    {
        Dictionary<string, List<ReferenceById>> categoryAndPosts = [];

        IEnumerable<CategoryInfo> categories = _pageManagementRepository.GetAllCategories();

        foreach (CategoryInfo category in categories)
        {
            var postReferences = _pageManagementRepository.GetAllPosts(category.Slug)
                .Select(post => new ReferenceById()
                {
                    Id = post.Id,
                    Name = post.Name
                }).ToList();

            categoryAndPosts.Add(category.Name, postReferences);
        }

        return categoryAndPosts;
    }

    [HttpPost("/posts")]
    public ActionResult<int> CreatePost([FromBody] PostInfo post)
    {
        if (post.Slug == null)
        {
            return StatusCode(400, "Slug must be specified.");
        }

        try
        {
            var postId = _pageManagementRepository.CreatePost(post);

            return Created($"/posts/{postId}", postId);
        }
        catch (InvalidOperationException)
        {
            _logger.LogError("Post with that slug already exists.");

            var existingPost = _pageManagementRepository.GetAllPosts()
                .First(existingPost => existingPost.Slug == post.Slug);

            return Conflict(new ConflictDetails()
            {
                ConflictingId = existingPost.Id
            });
        }
    }

    [HttpGet("/posts/{id}")]
    public ActionResult<PostInfo> GetPost(int id)
    {
        using var _ = _logger.BeginScope(new { PostId = id });

        _logger.LogInformation("Received request for post.");

        try
        {
            PostInfo post = _pageManagementRepository.GetPost(id);

            return Ok(post);
        }
        catch (ArgumentOutOfRangeException)
        {
            _logger.LogError("The request was for a post that does not exist.");

            return NotFound();
        }
    }

    [HttpPut("/posts/{id}")]
    public ActionResult<CategoryInfo> UpdatePost([FromRoute] int id, [FromBody] PostInfo post)
    {
        if (post.Slug == null)
        {
            return StatusCode(400, "Slug must be specified.");
        }

        if (_pageManagementRepository.DoesPageWithSlugExist(post.Slug) == false)
            return NotFound();

        post.Id = id;

        _ = _pageManagementRepository.UpdatePost(post);

        return NoContent();
    }
}
