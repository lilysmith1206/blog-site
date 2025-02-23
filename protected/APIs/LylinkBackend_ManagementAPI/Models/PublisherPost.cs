namespace LylinkBackend_ManagementAPI.Models
{
    public class PublisherPost
    {
        public string? Slug { get; set; }

        public string? Title { get; set; }

        public string? Name { get; set; }

        public string? Keywords { get; set; }

        public string? Description { get; set; }

        public string? Body { get; set; }

        public int? ParentId { get; set; }

        public bool? IsDraft { get; set; }
    }
}
