using Lylink.Management.Shared;

namespace Lylink.Site.Management.Models
{
    public struct Publisher
    {
        public bool NavigatedFromFormSubmit { get; set; }

        public IEnumerable<ReferenceById> Categories { get; set; }

        public Dictionary<string, List<ReferenceById>> CategoryPosts { get; set; }
    }
}
