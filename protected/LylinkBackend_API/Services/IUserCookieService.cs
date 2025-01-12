namespace LylinkBackend_API.Services
{
    public interface IUserCookieService
    {
        public string? GetUserAuthenticationToken(IRequestCookieCollection cookies);

        public string? GetVisitorId(IRequestCookieCollection cookies);
    }
}
