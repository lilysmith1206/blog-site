namespace LylinkBackend_API.Models
{
    public struct CategoryPage
    {
        public IEnumerable<PageLink> ParentHeader { get; set; }

        public IEnumerable<PageLink> SubCategories { get; set; }

        public IEnumerable<PageLink> Posts { get; set; }

        public string PageName { get; set; }

        public string Body { get; set; }

        public string Keywords { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }
    }
}
