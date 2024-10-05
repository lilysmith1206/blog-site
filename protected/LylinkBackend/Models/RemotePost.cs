namespace LylinkBackend_API.Models
{
    public class RemotePost
    {
        public string? Slug { get; set; }

        public string? Title { get; set; }

        public string? DateModifiedISO { get; set; }

        public string? DateCreatedISO { get; set; }

        public string? ParentSlug { get; set; }

        public string? Name { get; set; }

        public string? Keywords { get; set; }

        public string? Description { get; set; }

        public string? Body { get; set; }
    }
}