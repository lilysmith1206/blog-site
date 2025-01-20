using LylinkBackend_DatabaseAccessLayer.BusinessModels;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public interface IPageRepository
    {
        public PostPage? GetPost(string slug);

        public IEnumerable<KeyValuePair<string, string>> GetRecentlyUpdatedPostInfos(int amount);

        public CategoryPage? GetCategory(string slug);
    }
}
