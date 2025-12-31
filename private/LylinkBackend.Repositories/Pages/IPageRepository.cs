using LylinkShared.Models;

namespace LylinkBackend.Repositories.Pages
{
    public interface IPageRepository
    {
        public PostPage? GetPost(string slug);

        public IEnumerable<PageLink> GetRecentlyUpdatedPostInfos(int amount);

        public CategoryPage? GetCategory(string slug);
    }
}
