namespace LylinkBackend_API.Models
{
    public struct Publisher
    {
        public bool NavigatedFromFormSubmit { get; set; }

        public IEnumerable<PageLink> Categories { get; set; }

        public Dictionary<string, IEnumerable<PageLink>> CategoryPosts { get; set; }
    }
}
