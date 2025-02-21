namespace LylinkBackend_API.Models
{
    public struct ManagementToken
    {
        public Guid Id { get; set; }

        public DateTime TokenExpiration { get; set; }
    }
}