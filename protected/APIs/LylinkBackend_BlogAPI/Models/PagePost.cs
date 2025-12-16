using LylinkBackend_DatabaseAccessLayer.BusinessModels;

namespace LylinkBackend_API.Models
{
    public class PagePost : BasePage
    {
        public string? EditorName { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
