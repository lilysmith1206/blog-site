namespace LylinkBackend_API.Models
{
    public struct Publisher
    {
        public bool NavigatedFromFormSubmit { get; set; }

        public IEnumerable<string?> AvailableCategories { get; set; }

        public IEnumerable<string?> AvailableSlugs { get; set; }
    }
}
