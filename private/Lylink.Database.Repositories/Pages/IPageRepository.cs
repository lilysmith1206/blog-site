using Lylink.Shared.Models;

namespace Lylink.Database.Repositories.Pages
{
    public interface IPageRepository
    {
        public PostPage? GetPost(string slug);

        public IEnumerable<PageLink> GetRecentlyUpdatedPostInfos(int amount);

        public CategoryPage? GetCategory(string slug);
    }
}
