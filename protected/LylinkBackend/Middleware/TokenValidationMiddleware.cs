using LylinkBackend_API.Models;
using Microsoft.Extensions.Options;

namespace LylinkBackend_API.Middleware
{
    public class TokenValidationMiddleware(RequestDelegate next, IOptions<Authentication> authentication)
    {
        private readonly string[] RestrictedSlugs = ["/management", "/publisher", "/getPostFromSlug", "/savePost"]; 

        public async Task InvokeAsync(HttpContext context)
        {
            if (RestrictedSlugs.Contains(context.Request.Path.ToString()))
            {
                if (context.Request.Cookies.TryGetValue("token", out string? token) == false || authentication.Value.UserIds.Contains(token) == false)
                {
                    context.Response.Redirect("/403");

                    return;
                }
            }

            await next(context);
        }
    }
}
