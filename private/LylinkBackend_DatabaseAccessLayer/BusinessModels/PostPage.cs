namespace LylinkBackend_DatabaseAccessLayer.BusinessModels
{
    public struct PostPage
    {
        public string Slug;

        public string Title;

        public IEnumerable<KeyValuePair<string, string>> Parents;

        public DateTime DateModified;

        public DateTime DateCreated;

        public bool IsDraft;

        public string Name;

        public string Keywords;

        public string Description;

        public string Body;
    }
}
