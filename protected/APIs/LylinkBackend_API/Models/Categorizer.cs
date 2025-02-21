using LylinkBackend_DatabaseAccessLayer.BusinessModels;

namespace LylinkBackend_API.Models
{
    public struct Categorizer
    {
        public IEnumerable<CategoryInfo> Categories { get; set; }
    }
}
