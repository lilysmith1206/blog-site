namespace LylinkBackend_API.Models
{
    public struct HeadContent
    {
        public bool NeedsTableStyling { get; set; }

        public bool NeedsEditorTooling { get; set; }

        public string Keywords { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }
    }
}
