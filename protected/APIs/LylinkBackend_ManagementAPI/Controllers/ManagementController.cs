using LylinkBackend_ManagementAPI.Models;
using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace LylinkBackend_ManagementAPI.Controllers
{
    [ApiController]
    [Route("Management")]
    public class ManagementController(IPageManagementRepository pageManagementRepository) : Controller
    {
        [HttpGet("/")]
        public IActionResult Management()
        {
            return base.View(nameof(Models.Management), new Management());
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
            Dictionary<string, IEnumerable<PostInfo>> categoryAndChildPosts = GetPostsOrganizedByCategoryName();

            return base.View(nameof(Models.Publisher), new Publisher()
            {
                Categories = pageManagementRepository.GetAllCategories(),
                CategoryPosts = categoryAndChildPosts,
            });
        }

        [HttpGet("/getPostFromId")]
        public IActionResult GetSlugPost([FromQuery] int id)
        {
            PostInfo post = pageManagementRepository.GetPost(id);

            return Ok(new PublisherPost()
            {
                Body = post.Body,
                Description = post.Description,
                IsDraft = post.IsDraft,
                Keywords = post.Keywords,
                Name = post.Name,
                ParentId = post.ParentId,
                Slug = post.Slug,
                Title = post.Title
            });
        }

        [HttpGet("/getPostCategoryFromId")]
        public IActionResult GetPostCategoryFromSlug([FromQuery] int categoryId)
        {
            CategoryInfo category = pageManagementRepository.GetCategory(categoryId);

            return Ok(new CategorizerCategory()
            {
                Body = category.Body,
                Description = category.Description,
                Keywords = category.Keywords,
                Name = category.Name,
                ParentId = category.ParentId,
                PostSortingMethod = category.PostSortingMethod,
                Slug = category.Slug,
                Title = category.Title,
            });
        }

        [HttpPost("/savePost")]
        public IActionResult SaveDraft([FromForm] PublisherPost publisherPostInfo)
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

                if (pageManagementRepository.DoesPageWithSlugExist(postInfo.Slug))
                {
                    pageManagementRepository.UpdatePost(postInfo);
                }
                else
                {
                    pageManagementRepository.CreatePost(postInfo);
                }

                return RedirectToAction("Publisher", "Management", new { successfulPostSubmit = true });
            }
            catch (Exception)
            {
                return StatusCode(500, $"Issue adding/updating post {publisherPostInfo.Name}");
            }
        }

        [HttpPost("/saveCategory")]
        public IActionResult SaveCategory([FromForm] CategorizerCategory categorizerCategory)
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

                if (pageManagementRepository.DoesPageWithSlugExist(categoryInfo.Slug))
                {
                    pageManagementRepository.UpdateCategory(categoryInfo);
                }
                else
                {
                    pageManagementRepository.CreateCategory(categoryInfo);
                }

                return RedirectToAction("Categorizer", "Management", new { successfulPostSubmit = true });
            }
            catch (Exception)
            {
                return StatusCode(500, $"Issue adding/updating post {categorizerCategory.Name}");
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
