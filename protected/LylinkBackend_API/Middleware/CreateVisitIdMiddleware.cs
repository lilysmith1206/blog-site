using LylinkBackend_API.Services;

namespace LylinkBackend_API.Middleware
{
    public class CreateVisitIdMiddleware(RequestDelegate next, IUserCookieService cookieRetrievalService)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            string? visitorId = cookieRetrievalService.GetVisitorId(context.Request.Cookies);

            if (visitorId == null)
            {
                visitorId = $"{GenerateDashlessGuid()}{GenerateDashlessGuid()}{GenerateDashlessGuid()}{GenerateDashlessGuid()}";

                context.Response.Cookies.Append("visitor_id", visitorId);

                context.Items["new_visitor_id"] = visitorId;
            }

            await next(context);
        }

        private static string GenerateDashlessGuid() => Guid.NewGuid().ToString().Replace("-", string.Empty);
    }
}
