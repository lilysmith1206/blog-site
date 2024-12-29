namespace LylinkBackend_API.Models
{
    public struct PageHeader
    {
        public IEnumerable<PageLink> ParentCategories { get; set; }

        public string Name { get; set; }
    }
}
