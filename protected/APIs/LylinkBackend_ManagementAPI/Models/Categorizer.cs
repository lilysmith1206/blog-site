using LylinkBackend_DatabaseAccessLayer.BusinessModels;

namespace LylinkBackend_ManagementAPI.Models
{
    public struct Categorizer
    {
        public IEnumerable<CategoryInfo> Categories { get; set; }
    }
}
