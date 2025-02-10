﻿using LylinkBackend_DatabaseAccessLayer.BusinessModels;

namespace LylinkBackend_API.Models
{
    public struct Publisher
    {
        public bool NavigatedFromFormSubmit { get; set; }

        public IEnumerable<CategoryInfo> Categories { get; set; }

        public Dictionary<string, IEnumerable<PostInfo>> CategoryPosts { get; set; }
    }
}
