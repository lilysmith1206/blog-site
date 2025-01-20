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

        public IEnumerable<KeyValuePair<string, string>> ParentCategories;

        public IEnumerable<KeyValuePair<string, string>> ChildrenCategories;

        public IEnumerable<KeyValuePair<string, string>> Posts;

        public PostSortingMethod PostSortingMethod;
    }
}
