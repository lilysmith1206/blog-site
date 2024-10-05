namespace LylinkBackend_API.Models
{
    public struct BasePageModel
    {
        public bool AddSuggestionTools { get; set; }

        public string Title { get; set; }
        
        public string Keywords { get; set; }
        
        public string Description { get; set; }

        public string ParentHeader { get; set; }

        public string PageName { get; set; }

        public string UpdatedDateTime { get; set; }

        public string Body { get; set; }
        
        public bool DoesPostContainTableElement { get; set; }
    }

}
