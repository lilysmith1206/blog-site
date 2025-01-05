
namespace LylinkBackend_API.Caches
{
    public class AccessTokenCache : IAccessTokenCache
    {
        private readonly Dictionary<Guid, DateTime> _accessTokens = new Dictionary<Guid, DateTime>();
        private readonly TimeSpan _tokenValidFor = TimeSpan.FromDays(1);

        public DateTime? AddAccessToken(Guid accessToken)
        {
            DateTime expiryDate = DateTime.UtcNow.Add(_tokenValidFor);

            _accessTokens.Add(accessToken, expiryDate);

            return expiryDate;
        }

        public bool VerifyAccessToken(Guid accessToken)
        {
            if (_accessTokens.TryGetValue(accessToken, out DateTime value) && value > DateTime.UtcNow)
            {
                return true;
            }
            else
            {
                _accessTokens.Remove(accessToken);

                return false;
            }
        }
    }
}
