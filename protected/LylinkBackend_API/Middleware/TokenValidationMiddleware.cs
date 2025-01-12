using LylinkBackend_API.Caches;
using LylinkBackend_API.Models;
using LylinkBackend_API.Services;
using Microsoft.Extensions.Options;

namespace LylinkBackend_API.Middleware
{
    public class TokenValidationMiddleware(
        RequestDelegate next,
        IVisitAnalyticsCache visitAnalyticsCache,
        IUserCookieService userCookieService,
        IOptions<Authentication> authentication)
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
                string? token = userCookieService.GetUserAuthenticationToken(context.Request.Cookies);

                if (token == null || authentication.Value.UserIds.Contains(token) == false)
                {
                    string visitorId = userCookieService.GetVisitorId(context.Request.Cookies)
                        ?? $"{GenerateDashlessGuid()}{GenerateDashlessGuid()}{GenerateDashlessGuid()}{GenerateDashlessGuid()}";

                    context.Response.Cookies.Append("visitor_id", visitorId);

                    context.Items["new_visitor_id"] = visitorId;

                    visitAnalyticsCache.QueueVisitAnalyticsForProcessing(requestedSlug, "403", visitorId);

                    context.Response.Redirect("/403");

                    return;
                }
            }

            await next(context);
        }

        private static string GenerateDashlessGuid() => Guid.NewGuid().ToString().Replace("-", string.Empty);
    }
}
