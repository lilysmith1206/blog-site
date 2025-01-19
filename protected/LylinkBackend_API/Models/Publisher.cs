namespace LylinkBackend_API.Models
{
    public struct Publisher
    {
        public bool NavigatedFromFormSubmit { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Categories { get; set; }

        public Dictionary<string, IEnumerable<KeyValuePair<string, string>>> CategoryPosts { get; set; }
    }
}
