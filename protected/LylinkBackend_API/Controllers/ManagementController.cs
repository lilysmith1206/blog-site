using LylinkBackend_API.Models;
using LylinkBackend_API.Services;
using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace LylinkBackend_API.Controllers
{
    [ApiController]
    [Route("Management")]
    public class ManagementController(IPageManagementRepository pageManagementRepository, IStylesheetManagementRepository stylesheetManagementRepository) : Controller
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
                Categories = pageManagementRepository.GetAllCategories()
                    .Where(category => category.Id != 6)
            });
        }

        [HttpGet("/publisher")]
        public IActionResult Publisher([FromQuery] bool? successfulPostSubmit)
        {
            Dictionary<string, IEnumerable<PostInfo>> categoryAndChildPosts = GetPostsOrganizedByCategoryName();

            return base.View(nameof(Models.Publisher), new Publisher()
            {
                NavigatedFromFormSubmit = successfulPostSubmit == true,
                Categories = pageManagementRepository.GetAllCategories(),
                CategoryPosts = categoryAndChildPosts,
            });
        }

        [HttpGet("/styler")]
        public IActionResult Styler()
        {
            return base.View(nameof(Models.Styler), new Styler()
            {
                Stylesheets = stylesheetManagementRepository.GetAllStylesheets() 
            });
        }

        [HttpGet("/getPostFromId")]
        public IActionResult GetSlugPost([FromQuery] int id)
        {
            PostInfo post = pageManagementRepository.GetPost(id);

            return Ok(post);
        }

        [HttpGet("/getPostCategoryFromId")]
        public IActionResult GetPostCategoryFromSlug([FromQuery] int categoryId)
        {
            CategoryInfo category = pageManagementRepository.GetCategory(categoryId);

            return Ok(category);
        }

        [HttpGet("/getStylesheetFromName")]
        public IActionResult GetStylesheetFromName([FromQuery] string name)
        {
            try
            {
                IEnumerable<CssRule> stylesheetData = stylesheetManagementRepository.RetrieveStylesheetData(name);

                return Ok(stylesheetData);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(404, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("/savePost")]
        public IActionResult SaveDraft([FromForm] PostInfo remotePost)
        {
            try
            {
                if (pageManagementRepository.DoesPageWithSlugExist(remotePost.Slug))
                {
                    pageManagementRepository.UpdatePost(remotePost);
                }
                else
                {
                    pageManagementRepository.CreatePost(remotePost);
                }

                return RedirectToAction("Publisher", "Management", new { successfulPostSubmit = true });
            }
            catch (Exception)
            {
                return StatusCode(500, $"Issue adding/updating post {remotePost.Name}");
            }
        }

        [HttpPost("/saveCategory")]
        public IActionResult SaveCategory([FromForm] CategoryInfo remoteCategory)
        {
            try
            {
                if (pageManagementRepository.DoesPageWithSlugExist(remoteCategory.Slug))
                {
                    pageManagementRepository.UpdateCategory(remoteCategory);
                }
                else
                {
                    pageManagementRepository.CreateCategory(remoteCategory);
                }

                return RedirectToAction("Categorizer", "Management", new { successfulPostSubmit = true });
            }
            catch (Exception)
            {
                return StatusCode(500, $"Issue adding/updating post {remoteCategory.Name}");
            }
        }

        [HttpPost("/saveStylesheet")]
        public IActionResult SaveStylesheet([FromQuery] string sheetName, [FromBody] List<CssRule> stylesheetData)
        {
            bool updated = stylesheetManagementRepository.UpdateStylesheet(sheetName, stylesheetData);

            if (updated)
            {
                return Ok();
            }
            else
            {
                return StatusCode(400, "Stylesheet does not exist.");
            }
        }

        private Dictionary<string, IEnumerable<PostInfo>> GetPostsOrganizedByCategoryName()
        {
            Dictionary<string, IEnumerable<PostInfo>> categoryAndPosts = [];

            IEnumerable<CategoryInfo> categories = pageManagementRepository.GetAllCategories();

            foreach (CategoryInfo category in categories)
            {
                categoryAndPosts.Add(category.Name, pageManagementRepository.GetAllPosts(category.Slug));
            }

            return categoryAndPosts;
        }
    }
}
