namespace LylinkBackend_API.Models
{
    public struct PageCategory
    {
        public IEnumerable<KeyValuePair<string, string>> ParentCategories { get; set; }

        public IEnumerable<KeyValuePair<string, string>> SubCategories { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Posts { get; set; }

        public string PageName { get; set; }

        public string Body { get; set; }

        public string Keywords { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }
    }
}
