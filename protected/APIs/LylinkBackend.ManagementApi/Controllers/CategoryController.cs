using LylinkBackend.ManagementShared;
using LylinkBackend_DatabaseAccessLayer.Services;
using LylinkShared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            });

        return Ok(categories);
    }

    [HttpPost("/categories")]
    public ActionResult<int> CreateCategory([FromBody] CategoryInfo category)
    {
        int categoryId = _pageManagementRepository.CreateCategory(category);

        return Created($"/categories/{categoryId}", categoryId);
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
        if (category.Slug == null)
        {
            return StatusCode(400, "Slug must be specified.");
        }

        category.Id = id;

        _ = _pageManagementRepository.UpdateCategory(category);

        return NoContent();
    }
}
