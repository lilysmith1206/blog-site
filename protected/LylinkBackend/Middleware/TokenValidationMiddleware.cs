using LylinkBackend_API.Caches;

namespace LylinkBackend_API.Middleware
{
    public class TokenValidationMiddleware(RequestDelegate next, IAccessTokenCache accessTokenCache)
    {
        private readonly string[] RestrictedSlugs = ["/management", "/publisher"]; 

        public async Task InvokeAsync(HttpContext context)
        {
            if (RestrictedSlugs.Contains(context.Request.Path.ToString()))
            {
                if (context.Request.Cookies.TryGetValue("accessToken", out string? token) == false || IsValidToken(token) == false)
                {
                    context.Response.Redirect("/403");

                    return;
                }
            }

            await next(context);
        }

        private bool IsValidToken(string? token)
        {
            if (Guid.TryParse(token, out Guid guidToken))
            {
                return accessTokenCache.VerifyAccessToken(guidToken);
            }

            return false;
        }
    }

}
