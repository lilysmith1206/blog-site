using LylinkBackend_DatabaseAccessLayer.BusinessModels;

namespace LylinkBackend_API.Models
{
    public struct Styler
    {
        public IEnumerable<PageLink> Stylesheets { get; set; }
    }
}
