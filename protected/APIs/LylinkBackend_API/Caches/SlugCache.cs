namespace LylinkBackend_API.Caches
{
    public class SlugCache : ISlugCache
    {
        private List<string?> _postSlugs = [];
        private List<string?> _categorySlugs = [];

        private readonly object _lock = new();

        public IReadOnlyList<string?> GetPostSlugs()
        {
            lock (_lock)
            {
                return _postSlugs.AsReadOnly();
            }
        }

        public IReadOnlyList<string?> GetCategorySlugs()
        {
            lock (_lock)
            {
                return _categorySlugs.AsReadOnly();
            }
        }

        public void UpdatePostSlugs(IEnumerable<string?> slugs)
        {
            lock (_lock)
            {
                _postSlugs = slugs.ToList();
            }
        }

        public void UpdateCategorySlugs(IEnumerable<string?> slugs)
        {
            lock (_lock)
            {
                _categorySlugs = slugs.ToList();
            }
        }
    }

}
