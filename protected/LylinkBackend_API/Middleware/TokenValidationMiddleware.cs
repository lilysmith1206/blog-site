using LylinkBackend_API.Caches;
using LylinkBackend_API.Models;
using Microsoft.Extensions.Options;

namespace LylinkBackend_API.Middleware
{
    public class TokenValidationMiddleware(RequestDelegate next, IVisitAnalyticsCache visitAnalyticsCache, IOptions<Authentication> authentication)
    {
        private readonly string[] RestrictedSlugs = [
            "/management",
            "/categorizer",
            "/publisher",
            "/getPostFromSlug",
            "/savePost",
            "/getPostCategoryFromId",
            "/saveCategory"]; 

        public async Task InvokeAsync(HttpContext context)
        {
            string requestedSlug = context.Request.Path.ToString();

            if (RestrictedSlugs.Contains(requestedSlug))
            {
                if (context.Request.Cookies.TryGetValue("token", out string? token) == false || authentication.Value.UserIds.Contains(token) == false)
                {
                    context.Request.Cookies.TryGetValue("visitor_id", out string? visitorId);

                    visitAnalyticsCache.QueueVisitAnalyticsForProcessing(requestedSlug, "403", visitorId);

                    context.Response.Redirect("/403");

                    return;
                }
            }

            await next(context);
        }
    }
}
