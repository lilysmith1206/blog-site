namespace LylinkBackend_API.Caches
{
    public interface IAccessTokenCache
    {
        /// <summary>
        /// Validates a GUID token can be used to access restricted pages.
        /// </summary>
        /// <param name="accessToken">GUID access token to verify.</param>
        /// <returns>True if valid, false is not.</returns>
        bool VerifyAccessToken(Guid accessToken);

        /// <summary>
        /// Adds an access token as a token that can be used to access restricted pages.
        /// </summary>
        /// <param name="accessToken">The GUID to add as a valid access token.</param>
        /// <returns>The DateTime the token will expire. Null if adding was unsuccessful.</returns>
        DateTime? AddAccessToken(Guid accessToken);
    }
}
