using LylinkBackend_API.Models;
using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace LylinkBackend_API.Controllers
{
    [ApiController]
    [Route("Management")]
    public class ManagementController(IPageManagementRepository pageManagementRepository) : Controller
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
                CategoryLinks = pageManagementRepository.GetAllCategories()
                    .Where(category => category.Key != "6")
            });
        }

        [HttpGet("/publisher")]
        public IActionResult Publisher([FromQuery] bool? successfulPostSubmit)
        {
            Dictionary<string, IEnumerable<KeyValuePair<string, string>>> categoryAndChildPosts = GetPostsOrganizedByCategoryName();

            return base.View(nameof(Models.Publisher), new Publisher()
            {
                NavigatedFromFormSubmit = successfulPostSubmit == true,
                Categories = pageManagementRepository.GetAllCategories(),
                CategoryPosts = categoryAndChildPosts,
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

        private Dictionary<string, IEnumerable<KeyValuePair<string, string>>> GetPostsOrganizedByCategoryName()
        {
            Dictionary<string, IEnumerable<KeyValuePair<string, string>>> categoryAndPosts = [];

            IEnumerable<KeyValuePair<int, string>> categories = pageManagementRepository.GetAllCategories()
                .Select(category => KeyValuePair.Create(int.Parse(category.Key), category.Value));
            
            foreach (KeyValuePair<int, string> category in categories)
            {
                categoryAndPosts.Add(category.Value, pageManagementRepository.GetAllPosts(category.Key));
            }

            return categoryAndPosts;
        }
    }
}
