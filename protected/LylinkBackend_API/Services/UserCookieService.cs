namespace LylinkBackend_API.Services
{
    public class UserCookieService : IUserCookieService
    {
        public string? GetUserAuthenticationToken(IRequestCookieCollection cookies)
        {
            _ = cookies.TryGetValue("token", out string? token);

            return token;
        }

        public string? GetVisitorId(IRequestCookieCollection cookies)
        {
            _ = cookies.TryGetValue("visitor_id", out string? visitorId);

            return visitorId;
        }
    }
}
