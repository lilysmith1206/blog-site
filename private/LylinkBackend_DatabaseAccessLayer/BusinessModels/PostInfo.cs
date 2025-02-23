namespace LylinkBackend_DatabaseAccessLayer.BusinessModels
{
    public class PostInfo
    {
        public int Id { get; set; }

        public string Slug { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Keywords { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public int? ParentId { get; set; }

        public bool IsDraft { get; set; }
    }
}