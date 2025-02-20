namespace LylinkBackend_API.Models
{
    public struct CssRule
    {
        public IEnumerable<string> Selectors { get; set; }

        public IEnumerable<CssDeclaration> Declarations { get; set; }
    }
}
