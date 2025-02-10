using LylinkBackend_DatabaseAccessLayer.BusinessModels;

namespace LylinkBackend_API.Models
{
    public struct PagePost
    {
        public string? EditorName { get; set; }

        public DateTime DateUpdated { get; set; }

        public IEnumerable<PageLink> Parents { get; set; }

        public string PageName { get; set; }

        public string Body { get; set; }

        public string Keywords { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }
    }
}
