using LylinkBackend.ManagementShared;

namespace LylinkFrontend.ManagementSite.Models
{
    public struct Categorizer
    {
        public IEnumerable<ReferenceById> Categories { get; set; }
    }
}
