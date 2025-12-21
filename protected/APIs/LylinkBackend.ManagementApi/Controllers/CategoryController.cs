using LylinkBackend.ManagementShared;
using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkBackend_DatabaseAccessLayer.Services;
using LylinkShared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace LylinkBackend.ManagementApi.Controllers;

[ApiController]
[Authorize]
[Route("/[controller]")]
public class CategoryController : Controller
{
    private readonly ILogger<CategoryController> _logger;
    private readonly IPageManagementRepository _pageManagementRepository;

    public CategoryController(ILogger<CategoryController> logger, IPageManagementRepository pageManagementRepository)
    {
        _logger = logger;
        _pageManagementRepository = pageManagementRepository;
    }

    [HttpGet("/categories")]
    public ActionResult<IEnumerable<ReferenceById>> GetCategories()
    {
        IEnumerable<ReferenceById> categories = _pageManagementRepository.GetAllCategories()
            .Select(category => new ReferenceById()
            {
                Id = category.Id,
                Name = category.Name
            }).ToList();

        return Ok(categories);
    }

    [HttpPost("/categories")]
    public ActionResult<int> CreateCategory([FromBody] CategoryInfo category)
    {
        if (category.Slug is null)
            return BadRequest("Slug must be specified.");

        try
        {
            var categoryId = _pageManagementRepository.CreateCategory(category);

            return Created($"/categories/{categoryId}", categoryId);
        }
        catch (InvalidOperationException)
        {
            _logger.LogError("Post with that slug already exists.");

            var existingCategory = _pageManagementRepository.GetAllCategories()
                .First(existingCategory => existingCategory.Slug == category.Slug);

            return Conflict(new ConflictDetails()
            {
                ConflictingId = existingCategory.Id
            });
        }
    }

    [HttpGet("/categories/{id}")]
    public ActionResult<CategoryInfo> GetCategory(int id)
    {
        using var _ = _logger.BeginScope(new { CategoryId = id });

        _logger.LogInformation("Received request for category.");

        try
        {
            CategoryInfo category = _pageManagementRepository.GetCategory(id);
     
            return Ok(category);
        }
        catch (ArgumentOutOfRangeException)
        {
            _logger.LogError("The request was for a category that does not exist.");

            return NotFound();
        }
    }

    [HttpPut("/categories/{id}")]
    public ActionResult UpdateCategory([FromRoute] int id, [FromBody] CategoryInfo category)
    {
        if (category.Slug is null)
            return BadRequest("Slug must be specified.");

        if (_pageManagementRepository.DoesPageWithSlugExist(category.Slug) == false)
            return NotFound();

        category.Id = id;

        _ = _pageManagementRepository.UpdateCategory(category);

        return NoContent();
    }
}
