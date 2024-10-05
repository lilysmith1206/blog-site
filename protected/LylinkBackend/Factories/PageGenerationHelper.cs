using LylinkBackend_API.Models;
using LylinkBackend_API.Renderers;
using LylinkBackend_Database.Models;
using LylinkBackend_Database.Services;
using System.Text.RegularExpressions;

namespace LylinkBackend_API.Factories
{
    public abstract class PageGenerationHelper(IDatabaseService databaseService, IRazorViewToStringRenderer renderer)
    {
        protected async Task<string> GeneratePage(BasePageModel pageModel)
        {
            return await renderer.RenderViewToStringAsync("Views/BasePage.cshtml", pageModel);
        }

        protected string BuildParentHeader(string parentId)
        {
            List<PostHierarchy> parents = databaseService.GetParentCategories(parentId);
            PostHierarchy firstParent = parents.Last();

            parents.RemoveAt(parents.Count - 1);

            static string IndexSlugCheck(string? slug) => string.IsNullOrEmpty(slug) ? "/" : slug;

            parents.Reverse();

            var parentsHeader = @$"<li><a href=""{IndexSlugCheck(firstParent?.Slug)}"">{firstParent?.Name?.ToLower()}</a></li>";

            foreach (PostHierarchy parent in parents)
            {
                parentsHeader += @$"<li><a href=""{IndexSlugCheck(parent.Slug)}"">{parent?.Name?.ToLower()}</a></li>";
            }

            return @$"<nav class=""breadcrumb""><ul>{parentsHeader}</ul></nav>";
        }

        protected static bool CheckPostBodyForTableElement(string? postBody)
        {
            Regex tableTagRegex = new(@"<table\b[^>]*>", RegexOptions.IgnoreCase);

            return tableTagRegex.IsMatch(postBody ?? string.Empty);
        }

        protected static string ConvertDateTimeToWebsiteFormat(DateTime dateTime)
        {
            string updateTime = $"{dateTime.Hour}:{dateTime.Minute:D2}";
            string updateDate = $"{dateTime.Month}/{dateTime.Day}/{dateTime.Year}";

            return $"Updated {updateTime} on ${updateDate}";
        }

    }
}
