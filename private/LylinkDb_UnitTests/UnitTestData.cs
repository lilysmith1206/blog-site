using LylinkBackend_DatabaseAccessLayer.Models; 

namespace LylinkBackend_DatabaseAccessLayer_UnitTests
{
    // So there's no datetime issue on miniscule processing speed issues.
    public static class Dates
    {
        public static DateTime CurrentDateTime = DateTime.Now;
    }

    public static class DatabaseUnitTestData
    {
        public static LylinkBackend_DatabaseAccessLayer.Models.PostSortingMethod ByDateCreatedAscendingSort
        {
            get
            {
                return new LylinkBackend_DatabaseAccessLayer.Models.PostSortingMethod
                {
                    Id = (int)LylinkBackend_DatabaseAccessLayer.BusinessModels.PostSortingMethod.ByDateCreatedAscending,
                    SortingName = LylinkBackend_DatabaseAccessLayer.BusinessModels.PostSortingMethod.ByDateCreatedAscending.ToString(),
                };
            }
        }

        public static LylinkBackend_DatabaseAccessLayer.Models.PostSortingMethod ByDateCreatedDescendingSort
        {
            get
            {
                return new LylinkBackend_DatabaseAccessLayer.Models.PostSortingMethod
                {
                    Id = (int)LylinkBackend_DatabaseAccessLayer.BusinessModels.PostSortingMethod.ByDateCreatedDescending,
                    SortingName = LylinkBackend_DatabaseAccessLayer.BusinessModels.PostSortingMethod.ByDateCreatedDescending.ToString(),
                };
            }
        }

        public static LylinkBackend_DatabaseAccessLayer.Models.PostSortingMethod ByDateModifiedAscendingSort
        {
            get
            {
                return new LylinkBackend_DatabaseAccessLayer.Models.PostSortingMethod
                {
                    Id = (int)LylinkBackend_DatabaseAccessLayer.BusinessModels.PostSortingMethod.ByDateModifiedAscending,
                    SortingName = LylinkBackend_DatabaseAccessLayer.BusinessModels.PostSortingMethod.ByDateModifiedAscending.ToString(),
                };
            }
        }

        public static LylinkBackend_DatabaseAccessLayer.Models.PostSortingMethod ByDateModifiedDescendingSort
        {
            get
            {
                return new LylinkBackend_DatabaseAccessLayer.Models.PostSortingMethod
                {
                    Id = (int)LylinkBackend_DatabaseAccessLayer.BusinessModels.PostSortingMethod.ByDateModifiedDescending,
                    SortingName = LylinkBackend_DatabaseAccessLayer.BusinessModels.PostSortingMethod.ByDateModifiedDescending.ToString(),
                };
            }
        }

        public static Post IndexPost1
        {
            get
            {
                return new Post
                {
                    Id = 1,
                    Slug = "index-post-1",
                    ParentId = 1,
                    DateCreated = Dates.CurrentDateTime.AddDays(-10),
                    DateModified = Dates.CurrentDateTime.AddDays(-5),
                    SlugNavigation = new Page()
                    {
                        Slug = "index-post-1",
                        Name = "Index Post 1",
                        Keywords = "index, about me",
                        Description = "About the blogger.",
                        Body = "This post introduces the blogger on the index level category.",
                        Title = "About Me"
                    },
                    IsDraft = false
                };
            }
        }

        public static PostCategory IndexCategory
        {
            get
            {
                return new PostCategory
                {
                    CategoryId = 1,
                    Slug = "/",
                    ParentId = null,
                    PostSortingMethodId = ByDateCreatedAscendingSort.Id,
                    SlugNavigation = new Page()
                    {
                        Slug = "/",
                        Description = "Index category description",
                        Keywords = "Index category keywords",
                        Body = "index body",
                        Name = "Index",
                        Title = "Index Title"
                    }
                };
            }
        }

        public static Post TechPost1
        {
            get
            {
                return new Post
                {
                    Id = 2,
                    Slug = "tech-post-1",
                    ParentId = 2,
                    DateCreated = Dates.CurrentDateTime.AddDays(-10),
                    DateModified = Dates.CurrentDateTime.AddDays(-5),
                    SlugNavigation = new Page()
                    {
                        Slug = "tech-post-1",
                        Title = "Introduction to Gadgets",
                        Name = "Tech Post 1",
                        Keywords = "tech, gadgets, intro",
                        Description = "An introductory guide to gadgets.",
                        Body = "This post introduces the basics of gadgets and their use in modern tech."
                    },
                    IsDraft = false
                };
            }
        }

        public static Post TechPost2
        {
            get
            {
                return new Post
                {
                    Id = 3,
                    Slug = "tech-post-2",
                    ParentId = 2,
                    DateCreated = Dates.CurrentDateTime.AddDays(-20),
                    DateModified = Dates.CurrentDateTime.AddDays(-10),
                    SlugNavigation = new Page()
                    {
                        Slug = "tech-post-2",
                        Title = "Future of AI",
                        Name = "Tech Post 2",
                        Keywords = "AI, future, technology",
                        Description = "Discussing the future of AI.",
                        Body = "This post explores the possible advancements and impacts of AI in the future."
                    },
                    IsDraft = false
                };
            }
        }

        public static Post TechPost3
        {
            get
            {
                return new Post
                {
                    Id = 4,
                    Slug = "tech-post-3",
                    ParentId = 2,
                    DateCreated = Dates.CurrentDateTime.AddDays(-30),
                    DateModified = Dates.CurrentDateTime.AddDays(-15),
                    SlugNavigation = new Page()
                    {
                        Slug = "tech-post-3",
                        Title = "Top Tech Trends 2024",
                        Name = "Tech Post 3",
                        Keywords = "tech trends, 2024",
                        Description = "The top technology trends to watch for in 2024.",
                        Body = "This post covers the latest technology trends expected to shape the industry in 2024."
                    },
                    IsDraft = false
                };
            }
        }

        public static PostCategory TechCategory
        {
            get
            {
                return new PostCategory
                {
                    CategoryId = 2,
                    Slug = "tech",
                    ParentId = 1,
                    PostSortingMethodId = ByDateCreatedAscendingSort.Id,
                    SlugNavigation = new Page()
                    {
                        Slug = "tech",
                        Description = "Technology category description",
                        Keywords = "tech, gadgets, computers",
                        Body = "This is the body of the tech category.",
                        Name = "Tech",
                        Title = "Tech Title"
                    }
                };
            }
        }

        public static Post MostRecentPostsPost1
        {
            get
            {
                return new Post
                {
                    Id = 5,
                    Slug = "most-recent-1",
                    ParentId = 3,
                    DateCreated = DateTime.MaxValue.AddYears(-1).AddDays(1),
                    DateModified = DateTime.MaxValue.AddYears(-1).AddDays(1),
                    SlugNavigation = new Page()
                    {
                        Slug = "most-recent-1",
                        Title = "Most Recent",
                        Name = "Most Recent 1",
                        Keywords = "most recent, 1",
                        Description = "The most recent posts, 1.",
                        Body = "This post is the least most recent one."
                    },
                    IsDraft = false
                };
            }
        }

        public static Post MostRecentPostsPost2
        {
            get
            {
                return new Post
                {
                    Id = 6,
                    Slug = "most-recent-2",
                    ParentId = 3,
                    DateCreated = DateTime.MaxValue.AddYears(-1).AddDays(2),
                    DateModified = DateTime.MaxValue.AddYears(-1).AddDays(2),
                    SlugNavigation = new Page()
                    {
                        Slug = "most-recent-2",
                        Title = "Most Recent",
                        Name = "Most Recent 2",
                        Keywords = "most recent, 2",
                        Description = "The most recent posts, 3.",
                        Body = "This post is the middle recent one."
                    },
                    IsDraft = false
                };
            }
        }

        public static Post MostRecentPostsPost3
        {
            get
            {
                return new Post
                {
                    Id = 7,
                    Slug = "most-recent-3",
                    ParentId = 3,
                    DateCreated = DateTime.MaxValue.AddYears(-1).AddDays(3),
                    DateModified = DateTime.MaxValue.AddYears(-1).AddDays(3),
                    SlugNavigation = new Page()
                    {
                        Slug = "most-recent-3",
                        Title = "Most Recent",
                        Name = "Most Recent 3",
                        Keywords = "most recent, 3",
                        Description = "The most recent posts, 3.",
                        Body = "This post is the most recent one."
                    },
                    IsDraft = false
                };
            }
        }

        public static PostCategory MostRecentPostsCategory
        {
            get
            {
                return new PostCategory
                {
                    CategoryId = 3,
                    Slug = "most-recent",
                    ParentId = 1,
                    PostSortingMethodId = ByDateCreatedAscendingSort.Id,
                    SlugNavigation = new Page()
                    {
                        Slug = "most-recent",
                        Description = "Technology category description",
                        Keywords = "tech, gadgets, computers",
                        Body = "This is the body of the tech category.",
                        Name = "Tech",
                        Title = "Tech Title"
                    }
                };
            }
        }

        public static Post PostSortingMethodPost1
        {
            get
            {
                return new Post
                {
                    Id = 8,
                    Slug = "post-sorting-method-1",
                    ParentId = 4,
                    DateCreated = DateTime.MaxValue.AddYears(-5).AddDays(2),
                    DateModified = DateTime.MaxValue.AddYears(-4).AddDays(3),
                    SlugNavigation = new Page()
                    {
                        Slug = "post-sorting-method-1",
                        Title = "Sorting Method Post 1",
                        Name = "Post Sorting Method 1",
                        Keywords = "sorting method, 1",
                        Description = "Testing sorting method posts, 1.",
                        Body = "This is the least recent post in the sorting method test."
                    },
                    IsDraft = false
                };
            }
        }

        public static Post PostSortingMethodPost2
        {
            get
            {
                return new Post
                {
                    Id = 9,
                    Slug = "post-sorting-method-2",
                    ParentId = 4,
                    DateCreated = DateTime.MaxValue.AddYears(-5).AddDays(3),
                    DateModified = DateTime.MaxValue.AddYears(-4).AddDays(4),
                    SlugNavigation = new Page()
                    {
                        Slug = "post-sorting-method-2",
                        Title = "Sorting Method Post 2",
                        Name = "Post Sorting Method 2",
                        Keywords = "sorting method, 2",
                        Description = "Testing sorting method posts, 2.",
                        Body = "This is the second least recent post in the sorting method test."
                    },
                    IsDraft = false
                };
            }
        }

        public static Post PostSortingMethodPost3
        {
            get
            {
                return new Post
                {
                    Id = 10,
                    Slug = "post-sorting-method-3",
                    ParentId = 4,
                    DateCreated = DateTime.MaxValue.AddYears(-5).AddDays(4),
                    DateModified = DateTime.MaxValue.AddYears(-4).AddDays(1),
                    SlugNavigation = new Page()
                    {
                        Slug = "post-sorting-method-3",
                        Title = "Sorting Method Post 3",
                        Name = "Post Sorting Method 3",
                        Keywords = "sorting method, 3",
                        Description = "Testing sorting method posts, 3.",
                        Body = "This is the second most recent post in the sorting method test."
                    },
                    IsDraft = false
                };
            }
        }

        public static Post PostSortingMethodPost4
        {
            get
            {
                return new Post
                {
                    Id = 11,
                    Slug = "post-sorting-method-4",
                    ParentId = 4,
                    DateCreated = DateTime.MaxValue.AddYears(-5).AddDays(1),
                    DateModified = DateTime.MaxValue.AddYears(-4).AddDays(2),
                    SlugNavigation = new Page()
                    {
                        Slug = "post-sorting-method-4",
                        Title = "Sorting Method Post 4",
                        Name = "Post Sorting Method 4",
                        Keywords = "sorting method, 4",
                        Description = "Testing sorting method posts, 4.",
                        Body = "This is the most recent post in the sorting method test."
                    },
                    IsDraft = false
                };
            }
        }

        public static PostCategory PostSortingMethodCategory
        {
            get
            {
                return new PostCategory
                {
                    CategoryId = 4,
                    Slug = "post-sorting-method",
                    ParentId = 1,
                    PostSortingMethodId = ByDateCreatedAscendingSort.Id,
                    SlugNavigation = new Page()
                    {
                        Slug = "post-sorting-method",
                        Description = "Sorting method category description",
                        Keywords = "sorting, method, posts",
                        Body = "This is the body of the sorting method category.",
                        Name = "Sorting Methods",
                        Title = "Sorting Methods Title"
                    }
                };
            }
        }

        public static List<PostCategory> Categories
        {
            get
            {
                return [IndexCategory, TechCategory, MostRecentPostsCategory, PostSortingMethodCategory];
            }
        }

        public static List<Post> Posts
        {
            get
            {
                return [
                    IndexPost1,
                    TechPost1,
                    TechPost2,
                    TechPost3,
                    MostRecentPostsPost1,
                    MostRecentPostsPost2,
                    MostRecentPostsPost3,
                    PostSortingMethodPost1,
                    PostSortingMethodPost2,
                    PostSortingMethodPost3,
                    PostSortingMethodPost4,
                ];
            }
        }

        public static List<Annotation> Annotations
        {
            get
            {
                return [];
            }
        }

        public static List<LylinkBackend_DatabaseAccessLayer.Models.PostSortingMethod> PostSortingMethods
        {
            get
            {
                return [
                    ByDateCreatedAscendingSort,
                    ByDateCreatedDescendingSort,
                    ByDateModifiedAscendingSort,
                    ByDateModifiedDescendingSort
                ];
            }
        }
    }
}
