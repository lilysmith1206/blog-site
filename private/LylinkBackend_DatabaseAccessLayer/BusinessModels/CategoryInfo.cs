namespace LylinkBackend_DatabaseAccessLayer.BusinessModels
{
    public class CategoryInfo
    {
        public int Id { get; set; }

        public required string Slug { get; set; }

        public required string Name { get; set; }

        public required string Title { get; set; }

        public required string Description { get; set; }

        public required string Keywords { get; set; }

        public required string Body { get; set; }

        public int? ParentId { get; set; }

        public required PostSortingMethod PostSortingMethod { get; set; }
    }
}
