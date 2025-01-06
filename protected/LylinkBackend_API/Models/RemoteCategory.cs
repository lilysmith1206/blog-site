namespace LylinkBackend_API.Models
{
    public class RemoteCategory
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Keywords { get; set; }

        public string? CategoryName { get; set; }

        public int? CategoryId { get; set; }

        public string? Slug { get; set; }

        public string? Body { get; set; }

        public int? ParentId { get; set; }

        public bool? UseDateCreatedForSorting { get; set; }
    }
}
