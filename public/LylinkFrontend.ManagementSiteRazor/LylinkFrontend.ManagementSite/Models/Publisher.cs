using LylinkBackend.ManagementShared;

namespace LylinkFrontend.ManagementSite.Models
{
    public struct Publisher
    {
        public bool NavigatedFromFormSubmit { get; set; }

        public IEnumerable<ReferenceById> Categories { get; set; }

        public Dictionary<string, List<ReferenceById>> CategoryPosts { get; set; }
    }
}
