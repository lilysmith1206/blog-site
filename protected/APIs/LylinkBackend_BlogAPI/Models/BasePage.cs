using LylinkBackend_DatabaseAccessLayer.BusinessModels;

namespace LylinkBackend_API.Models
{
    public class BasePage
    {
        public required string Body { get; set; }

        public required string Description { get; set; }

        public required string Keywords { get; set; }

        public required string PageName { get; set; }

        public required string Title { get; set; }

        public required IEnumerable<PageLink> ParentCategories { get; set; }
    }
}