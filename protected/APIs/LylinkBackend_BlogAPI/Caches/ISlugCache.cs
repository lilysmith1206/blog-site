namespace LylinkBackend_API.Caches
{
    public interface ISlugCache
    {
        IReadOnlyList<string?> GetPostSlugs();

        IReadOnlyList<string?> GetCategorySlugs();

        void UpdatePostSlugs(IEnumerable<string?> slugs);

        void UpdateCategorySlugs(IEnumerable<string?> slugs);
    }
}
