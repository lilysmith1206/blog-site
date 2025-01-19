namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public interface ISlugRepository
    {
        public IEnumerable<string> GetPostSlugs();

        public IEnumerable<string> GetCategorySlugs();
    }
}
