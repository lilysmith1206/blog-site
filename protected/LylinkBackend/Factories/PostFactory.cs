using LylinkBackend_API.Factories;
using LylinkBackend_API.Models;
using LylinkBackend_API.Renderers;
using LylinkBackend_Database.Models;
using LylinkBackend_Database.Services;

namespace LylinkBackend.Handlers
{
    public class PostFactory(IDatabaseService databaseService, IRazorViewToStringRenderer renderer) : PageGenerationHelper(databaseService, renderer)
    {
        public async Task<string> GeneratePost(string slug, bool addSuggestionTools = false)
        {
            Post post = databaseService.GetPost(slug) ?? throw new NullReferenceException($"No post found for slug: {slug}");

            BasePageModel model = new BasePageModel()
            {
                AddSuggestionTools = addSuggestionTools,
                Body = post.Body ?? string.Empty,
                Description = post.Description ?? string.Empty,
                DoesPostContainTableElement = CheckPostBodyForTableElement(post.Body),
                Keywords = post.Keywords ?? string.Empty,
                PageName = post.Name ?? string.Empty,
                ParentHeader = BuildParentHeader(post.ParentId ?? string.Empty),
                Title = post.Title ?? string.Empty,
                UpdatedDateTime = ConvertDateTimeToWebsiteFormat(post.DateModified ?? DateTime.Now)
            };

            return await GeneratePage(model);
        }
    }
}
