﻿namespace LylinkBackend_API.Models
{
    public struct PostPage
    {
        public string? EditorName { get; set; }

        public DateTime DateUpdated { get; set; }

        public IEnumerable<PageLink> ParentCategories { get; set; }

        public string PageName { get; set; }

        public string Body { get; set; }

        public string Keywords { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }
    }
}
