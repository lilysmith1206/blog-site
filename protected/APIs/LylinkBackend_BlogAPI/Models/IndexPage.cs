using LylinkBackend_DatabaseAccessLayer.BusinessModels;

namespace LylinkBackend_API.Models
{
    public class IndexPage : BasePage
    {
        public required IEnumerable<PageLink> SubCategories { get; set; }

        public required IEnumerable<PageLink> Posts { get; set; }

        public required IEnumerable<PageLink> MostRecentPosts { get; set; }
    }
}
