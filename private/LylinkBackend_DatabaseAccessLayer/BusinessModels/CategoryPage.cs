namespace LylinkBackend_DatabaseAccessLayer.BusinessModels
{
    public struct CategoryPage
    {
        public string Slug;

        public string Title;

        public string Name;

        public string Keywords;

        public string Description;

        public string Body;

        public IEnumerable<PageLink> ParentCategories;

        public IEnumerable<PageLink> ChildrenCategories;

        public IEnumerable<PageLink> Posts;

        public PostSortingMethod PostSortingMethod;
    }
}
