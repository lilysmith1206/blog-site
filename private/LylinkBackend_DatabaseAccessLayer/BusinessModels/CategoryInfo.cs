namespace LylinkBackend_DatabaseAccessLayer.BusinessModels
{
    public struct CategoryInfo
    {
        public int Id { get; set; }

        public string Slug { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Keywords { get; set; }

        public string Body { get; set; }

        public int? ParentId { get; set; }

        public bool UseDateCreatedForSorting { get; set; }
    }
}
