using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Models;

namespace LylinkDb_UnitTests
{
    // So there's no datetime issue on miniscule processing speed issues.
    public static class Dates
    {
        public static DateTime CurrentDateTime = DateTime.Now;
    }

    public static class DatabaseUnitTestData
    {
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
                    PostSortingMethodId = (int)LylinkBackend_DatabaseAccessLayer.BusinessModels.PostSortingMethod.ByDateCreatedDescending,
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
                    PostSortingMethodId = (int)LylinkBackend_DatabaseAccessLayer.BusinessModels.PostSortingMethod.ByDateCreatedDescending,
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
                    PostSortingMethodId = (int)LylinkBackend_DatabaseAccessLayer.BusinessModels.PostSortingMethod.ByDateCreatedDescending,
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

        public static List<PostCategory> Categories
        {
            get
            {
                return [IndexCategory, TechCategory, MostRecentPostsCategory];
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
                    MostRecentPostsPost3
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

    public static class UnitTestData
    {
        static UnitTestData()
        {
            IndexPostPage1.Parents = [KeyValuePair.Create(IndexCategoryPage.Slug, IndexCategoryPage.Name)];

            IndexCategoryPage.ParentCategories = [];
            IndexCategoryPage.ChildrenCategories = [KeyValuePair.Create(TechCategoryPage.Slug, TechCategoryPage.Name)];

            TechPostPage1.Parents = [KeyValuePair.Create(TechCategoryPage.Slug, TechCategoryPage.Name), KeyValuePair.Create(IndexCategoryPage.Slug, IndexCategoryPage.Name)];
            TechPostPage2.Parents = [KeyValuePair.Create(TechCategoryPage.Slug, TechCategoryPage.Name), KeyValuePair.Create(IndexCategoryPage.Slug, IndexCategoryPage.Name)];
            TechPostPage3.Parents = [KeyValuePair.Create(TechCategoryPage.Slug, TechCategoryPage.Name), KeyValuePair.Create(IndexCategoryPage.Slug, IndexCategoryPage.Name)];

            TechCategoryPage.ChildrenCategories = [];
            TechCategoryPage.Posts = [
                KeyValuePair.Create(TechPostPage1.Slug, TechPostPage1.Name),
                KeyValuePair.Create(TechPostPage2.Slug, TechPostPage2.Name),
                KeyValuePair.Create(TechPostPage3.Slug, TechPostPage3.Name)
            ];

            MostRecentPostsPostPage1.Parents = [KeyValuePair.Create(IndexCategoryPage.Slug, IndexCategoryPage.Name)];
            MostRecentPostsPostPage2.Parents = [KeyValuePair.Create(IndexCategoryPage.Slug, IndexCategoryPage.Name)];
            MostRecentPostsPostPage3.Parents = [KeyValuePair.Create(IndexCategoryPage.Slug, IndexCategoryPage.Name)];

            MostRecentPostsCategoryPage.ChildrenCategories = [];
            MostRecentPostsCategoryPage.Posts = [
                KeyValuePair.Create(MostRecentPostsPostPage1.Slug, MostRecentPostsPostPage1.Name),
                KeyValuePair.Create(MostRecentPostsPostPage2.Slug, MostRecentPostsPostPage2.Name),
                KeyValuePair.Create(MostRecentPostsPostPage3.Slug, MostRecentPostsPostPage3.Name)
            ];
        }

        public static PostPage IndexPostPage1 = new PostPage
        {
            Slug = DatabaseUnitTestData.IndexPost1.Slug,
            Title = DatabaseUnitTestData.IndexPost1.SlugNavigation.Title,
            DateCreated = DatabaseUnitTestData.IndexPost1.DateCreated,
            DateModified = DatabaseUnitTestData.IndexPost1.DateModified,
            Name = DatabaseUnitTestData.IndexPost1.SlugNavigation.Name,
            Keywords = DatabaseUnitTestData.IndexPost1.SlugNavigation.Keywords,
            Description = DatabaseUnitTestData.IndexPost1.SlugNavigation.Description,
            Body = DatabaseUnitTestData.IndexPost1.SlugNavigation.Body,
            IsDraft = DatabaseUnitTestData.IndexPost1.IsDraft
        };

        public static PostInfo IndexPostInfo1 = new PostInfo
        {
            Body = DatabaseUnitTestData.IndexPost1.SlugNavigation.Body,
            Description = DatabaseUnitTestData.IndexPost1.SlugNavigation.Description,
            Id = DatabaseUnitTestData.IndexPost1.Id,
            IsDraft = DatabaseUnitTestData.IndexPost1.IsDraft,
            Keywords = DatabaseUnitTestData.IndexPost1.SlugNavigation.Keywords,
            Name = DatabaseUnitTestData.IndexPost1.SlugNavigation.Name,
            ParentId = DatabaseUnitTestData.IndexPost1.ParentId,
            Slug = DatabaseUnitTestData.IndexPost1.Slug,
            Title = DatabaseUnitTestData.IndexPost1.SlugNavigation.Title
        };

        public static CategoryPage IndexCategoryPage = new CategoryPage
        {
            Slug = DatabaseUnitTestData.IndexCategory.SlugNavigation.Slug,
            PostSortingMethod = (LylinkBackend_DatabaseAccessLayer.BusinessModels.PostSortingMethod)DatabaseUnitTestData.IndexCategory.PostSortingMethodId!.Value,
            Description = DatabaseUnitTestData.IndexCategory.SlugNavigation.Description,
            Keywords = DatabaseUnitTestData.IndexCategory.SlugNavigation.Keywords,
            Body = DatabaseUnitTestData.IndexCategory.SlugNavigation.Body,
            Name = DatabaseUnitTestData.IndexCategory.SlugNavigation.Name,
            Title = DatabaseUnitTestData.IndexCategory.SlugNavigation.Title,
            Posts = [KeyValuePair.Create(IndexPostPage1.Slug, IndexPostPage1.Name)],
        };

        public static CategoryInfo IndexCategoryInfo = new CategoryInfo
        {
            Slug = DatabaseUnitTestData.IndexCategory.Slug,
            PostSortingMethod = (LylinkBackend_DatabaseAccessLayer.BusinessModels.PostSortingMethod)DatabaseUnitTestData.IndexCategory.PostSortingMethodId!.Value,
            Description = DatabaseUnitTestData.IndexCategory.SlugNavigation.Description,
            Keywords = DatabaseUnitTestData.IndexCategory.SlugNavigation.Keywords,
            Body = DatabaseUnitTestData.IndexCategory.SlugNavigation.Body,
            Name = DatabaseUnitTestData.IndexCategory.SlugNavigation.Name,
            Title = DatabaseUnitTestData.IndexCategory.SlugNavigation.Title,
            Id = DatabaseUnitTestData.IndexCategory.CategoryId,
            ParentId = DatabaseUnitTestData.IndexCategory.ParentId
        };

        public static PostPage TechPostPage1 = new PostPage
        {
            Slug = DatabaseUnitTestData.TechPost1.Slug,
            Title = DatabaseUnitTestData.TechPost1.SlugNavigation.Title,
            DateCreated = DatabaseUnitTestData.TechPost1.DateCreated,
            DateModified = DatabaseUnitTestData.TechPost1.DateModified,
            Name = DatabaseUnitTestData.TechPost1.SlugNavigation.Name,
            Keywords = DatabaseUnitTestData.TechPost1.SlugNavigation.Keywords,
            Description = DatabaseUnitTestData.TechPost1.SlugNavigation.Description,
            Body = DatabaseUnitTestData.TechPost1.SlugNavigation.Body,
            IsDraft = DatabaseUnitTestData.TechPost1.IsDraft,
        };

        public static PostInfo TechPostInfo1 = new PostInfo
        {
            Body = DatabaseUnitTestData.TechPost1.SlugNavigation.Body,
            Description = DatabaseUnitTestData.TechPost1.SlugNavigation.Description,
            Id = DatabaseUnitTestData.TechPost1.Id,
            IsDraft = DatabaseUnitTestData.TechPost1.IsDraft,
            Keywords = DatabaseUnitTestData.TechPost1.SlugNavigation.Keywords,
            Name = DatabaseUnitTestData.TechPost1.SlugNavigation.Name,
            ParentId = DatabaseUnitTestData.TechPost1.ParentId,
            Slug = DatabaseUnitTestData.TechPost1.Slug,
            Title = DatabaseUnitTestData.TechPost1.SlugNavigation.Title
        };

        public static PostPage TechPostPage2 = new PostPage
        {
            Slug = DatabaseUnitTestData.TechPost2.Slug,
            Title = DatabaseUnitTestData.TechPost2.SlugNavigation.Title,
            DateCreated = DatabaseUnitTestData.TechPost2.DateCreated,
            DateModified = DatabaseUnitTestData.TechPost2.DateModified,
            Name = DatabaseUnitTestData.TechPost2.SlugNavigation.Name,
            Keywords = DatabaseUnitTestData.TechPost2.SlugNavigation.Keywords,
            Description = DatabaseUnitTestData.TechPost2.SlugNavigation.Description,
            Body = DatabaseUnitTestData.TechPost2.SlugNavigation.Body,
            IsDraft = DatabaseUnitTestData.TechPost2.IsDraft,
        };

        public static PostInfo TechPostInfo2 = new PostInfo
        {
            Body = DatabaseUnitTestData.TechPost2.SlugNavigation.Body,
            Description = DatabaseUnitTestData.TechPost2.SlugNavigation.Description,
            Id = DatabaseUnitTestData.TechPost2.Id,
            IsDraft = DatabaseUnitTestData.TechPost2.IsDraft,
            Keywords = DatabaseUnitTestData.TechPost2.SlugNavigation.Keywords,
            Name = DatabaseUnitTestData.TechPost2.SlugNavigation.Name,
            ParentId = DatabaseUnitTestData.TechPost2.ParentId,
            Slug = DatabaseUnitTestData.TechPost2.Slug,
            Title = DatabaseUnitTestData.TechPost2.SlugNavigation.Title
        };

        public static PostPage TechPostPage3 = new PostPage
        {
            Slug = DatabaseUnitTestData.TechPost3.Slug,
            Title = DatabaseUnitTestData.TechPost3.SlugNavigation.Title,
            DateCreated = DatabaseUnitTestData.TechPost3.DateCreated,
            DateModified = DatabaseUnitTestData.TechPost3.DateModified,
            Name = DatabaseUnitTestData.TechPost3.SlugNavigation.Name,
            Keywords = DatabaseUnitTestData.TechPost3.SlugNavigation.Keywords,
            Description = DatabaseUnitTestData.TechPost3.SlugNavigation.Description,
            Body = DatabaseUnitTestData.TechPost3.SlugNavigation.Body,
            IsDraft = DatabaseUnitTestData.TechPost3.IsDraft,
        };

        public static PostInfo TechPostInfo3 = new PostInfo
        {
            Body = DatabaseUnitTestData.TechPost3.SlugNavigation.Body,
            Description = DatabaseUnitTestData.TechPost3.SlugNavigation.Description,
            Id = DatabaseUnitTestData.TechPost3.Id,
            IsDraft = DatabaseUnitTestData.TechPost3.IsDraft,
            Keywords = DatabaseUnitTestData.TechPost3.SlugNavigation.Keywords,
            Name = DatabaseUnitTestData.TechPost3.SlugNavigation.Name,
            ParentId = DatabaseUnitTestData.TechPost3.ParentId,
            Slug = DatabaseUnitTestData.TechPost3.Slug,
            Title = DatabaseUnitTestData.TechPost3.SlugNavigation.Title
        };

        public static CategoryPage TechCategoryPage = new CategoryPage
        {
            Slug = DatabaseUnitTestData.TechCategory.SlugNavigation.Slug,
            PostSortingMethod = (LylinkBackend_DatabaseAccessLayer.BusinessModels.PostSortingMethod)DatabaseUnitTestData.TechCategory.PostSortingMethodId!.Value,
            Description = DatabaseUnitTestData.TechCategory.SlugNavigation.Description,
            Keywords = DatabaseUnitTestData.TechCategory.SlugNavigation.Keywords,
            Body = DatabaseUnitTestData.TechCategory.SlugNavigation.Body,
            Name = DatabaseUnitTestData.TechCategory.SlugNavigation.Name,
            Title = DatabaseUnitTestData.TechCategory.SlugNavigation.Title,
            ParentCategories = [KeyValuePair.Create(IndexCategoryPage.Slug, IndexCategoryPage.Name)]
        };

        public static CategoryInfo TechCategoryInfo = new CategoryInfo
        {
            Slug = DatabaseUnitTestData.TechCategory.Slug,
            PostSortingMethod = (LylinkBackend_DatabaseAccessLayer.BusinessModels.PostSortingMethod)DatabaseUnitTestData.TechCategory.PostSortingMethodId!.Value,
            Description = DatabaseUnitTestData.TechCategory.SlugNavigation.Description,
            Keywords = DatabaseUnitTestData.TechCategory.SlugNavigation.Keywords,
            Body = DatabaseUnitTestData.TechCategory.SlugNavigation.Body,
            Name = DatabaseUnitTestData.TechCategory.SlugNavigation.Name,
            Title = DatabaseUnitTestData.TechCategory.SlugNavigation.Title,
            Id = DatabaseUnitTestData.TechCategory.CategoryId,
            ParentId = DatabaseUnitTestData.TechCategory.ParentId
        };

        public static PostPage MostRecentPostsPostPage1 = new PostPage
        {
            Slug = DatabaseUnitTestData.MostRecentPostsPost1.Slug,
            Title = DatabaseUnitTestData.MostRecentPostsPost1.SlugNavigation.Title,
            DateCreated = DatabaseUnitTestData.MostRecentPostsPost1.DateCreated,
            DateModified = DatabaseUnitTestData.MostRecentPostsPost1.DateModified,
            Name = DatabaseUnitTestData.MostRecentPostsPost1.SlugNavigation.Name,
            Keywords = DatabaseUnitTestData.MostRecentPostsPost1.SlugNavigation.Keywords,
            Description = DatabaseUnitTestData.MostRecentPostsPost1.SlugNavigation.Description,
            Body = DatabaseUnitTestData.MostRecentPostsPost1.SlugNavigation.Body,
            IsDraft = DatabaseUnitTestData.MostRecentPostsPost1.IsDraft,
        };

        public static PostInfo MostRecentPostsPostInfo1 = new PostInfo
        {
            Body = DatabaseUnitTestData.MostRecentPostsPost1.SlugNavigation.Body,
            Description = DatabaseUnitTestData.MostRecentPostsPost1.SlugNavigation.Description,
            Id = DatabaseUnitTestData.MostRecentPostsPost1.Id,
            IsDraft = DatabaseUnitTestData.MostRecentPostsPost1.IsDraft,
            Keywords = DatabaseUnitTestData.MostRecentPostsPost1.SlugNavigation.Keywords,
            Name = DatabaseUnitTestData.MostRecentPostsPost1.SlugNavigation.Name,
            ParentId = DatabaseUnitTestData.MostRecentPostsPost1.ParentId,
            Slug = DatabaseUnitTestData.MostRecentPostsPost1.Slug,
            Title = DatabaseUnitTestData.MostRecentPostsPost1.SlugNavigation.Title
        };

        public static PostPage MostRecentPostsPostPage2 = new PostPage
        {
            Slug = DatabaseUnitTestData.MostRecentPostsPost2.Slug,
            Title = DatabaseUnitTestData.MostRecentPostsPost2.SlugNavigation.Title,
            DateCreated = DatabaseUnitTestData.MostRecentPostsPost2.DateCreated,
            DateModified = DatabaseUnitTestData.MostRecentPostsPost2.DateModified,
            Name = DatabaseUnitTestData.MostRecentPostsPost2.SlugNavigation.Name,
            Keywords = DatabaseUnitTestData.MostRecentPostsPost2.SlugNavigation.Keywords,
            Description = DatabaseUnitTestData.MostRecentPostsPost2.SlugNavigation.Description,
            Body = DatabaseUnitTestData.MostRecentPostsPost2.SlugNavigation.Body,
            IsDraft = DatabaseUnitTestData.MostRecentPostsPost2.IsDraft,
        };

        public static PostInfo MostRecentPostsPostInfo2 = new PostInfo
        {
            Body = DatabaseUnitTestData.MostRecentPostsPost2.SlugNavigation.Body,
            Description = DatabaseUnitTestData.MostRecentPostsPost2.SlugNavigation.Description,
            Id = DatabaseUnitTestData.MostRecentPostsPost2.Id,
            IsDraft = DatabaseUnitTestData.MostRecentPostsPost2.IsDraft,
            Keywords = DatabaseUnitTestData.MostRecentPostsPost2.SlugNavigation.Keywords,
            Name = DatabaseUnitTestData.MostRecentPostsPost2.SlugNavigation.Name,
            ParentId = DatabaseUnitTestData.MostRecentPostsPost2.ParentId,
            Slug = DatabaseUnitTestData.MostRecentPostsPost2.Slug,
            Title = DatabaseUnitTestData.MostRecentPostsPost2.SlugNavigation.Title
        };

        public static PostPage MostRecentPostsPostPage3 = new PostPage
        {
            Slug = DatabaseUnitTestData.MostRecentPostsPost3.Slug,
            Title = DatabaseUnitTestData.MostRecentPostsPost3.SlugNavigation.Title,
            DateCreated = DatabaseUnitTestData.MostRecentPostsPost3.DateCreated,
            DateModified = DatabaseUnitTestData.MostRecentPostsPost3.DateModified,
            Name = DatabaseUnitTestData.MostRecentPostsPost3.SlugNavigation.Name,
            Keywords = DatabaseUnitTestData.MostRecentPostsPost3.SlugNavigation.Keywords,
            Description = DatabaseUnitTestData.MostRecentPostsPost3.SlugNavigation.Description,
            Body = DatabaseUnitTestData.MostRecentPostsPost3.SlugNavigation.Body,
            IsDraft = DatabaseUnitTestData.MostRecentPostsPost3.IsDraft,
        };

        public static PostInfo MostRecentPostsPostInfo3 = new PostInfo
        {
            Body = DatabaseUnitTestData.MostRecentPostsPost3.SlugNavigation.Body,
            Description = DatabaseUnitTestData.MostRecentPostsPost3.SlugNavigation.Description,
            Id = DatabaseUnitTestData.MostRecentPostsPost3.Id,
            IsDraft = DatabaseUnitTestData.MostRecentPostsPost3.IsDraft,
            Keywords = DatabaseUnitTestData.MostRecentPostsPost3.SlugNavigation.Keywords,
            Name = DatabaseUnitTestData.MostRecentPostsPost3.SlugNavigation.Name,
            ParentId = DatabaseUnitTestData.MostRecentPostsPost3.ParentId,
            Slug = DatabaseUnitTestData.MostRecentPostsPost3.Slug,
            Title = DatabaseUnitTestData.MostRecentPostsPost3.SlugNavigation.Title
        };

        public static CategoryPage MostRecentPostsCategoryPage = new CategoryPage
        {
            Slug = DatabaseUnitTestData.MostRecentPostsCategory.SlugNavigation.Slug,
            PostSortingMethod = (LylinkBackend_DatabaseAccessLayer.BusinessModels.PostSortingMethod)DatabaseUnitTestData.MostRecentPostsCategory.PostSortingMethodId!.Value,
            Description = DatabaseUnitTestData.MostRecentPostsCategory.SlugNavigation.Description,
            Keywords = DatabaseUnitTestData.MostRecentPostsCategory.SlugNavigation.Keywords,
            Body = DatabaseUnitTestData.MostRecentPostsCategory.SlugNavigation.Body,
            Name = DatabaseUnitTestData.MostRecentPostsCategory.SlugNavigation.Name,
            Title = DatabaseUnitTestData.MostRecentPostsCategory.SlugNavigation.Title,
            ParentCategories = [KeyValuePair.Create(IndexCategoryPage.Slug, IndexCategoryPage.Name)]
        };

        public static CategoryInfo MostRecentPostsCategoryInfo = new CategoryInfo
        {
            Slug = DatabaseUnitTestData.MostRecentPostsCategory.Slug,
            PostSortingMethod = (LylinkBackend_DatabaseAccessLayer.BusinessModels.PostSortingMethod)DatabaseUnitTestData.MostRecentPostsCategory.PostSortingMethodId!.Value,
            Description = DatabaseUnitTestData.MostRecentPostsCategory.SlugNavigation.Description,
            Keywords = DatabaseUnitTestData.MostRecentPostsCategory.SlugNavigation.Keywords,
            Body = DatabaseUnitTestData.MostRecentPostsCategory.SlugNavigation.Body,
            Name = DatabaseUnitTestData.MostRecentPostsCategory.SlugNavigation.Name,
            Title = DatabaseUnitTestData.MostRecentPostsCategory.SlugNavigation.Title,
            Id = DatabaseUnitTestData.MostRecentPostsCategory.CategoryId,
            ParentId = DatabaseUnitTestData.MostRecentPostsCategory.ParentId
        };

        /*
        public static Annotation Annotation1 = new Annotation() { AnnotationContent = "Annotation Content 1", EditorName = "JohnSmith", Id = "f3a5c3e2-166b-495a-827a-3314b82b130b", Slug = "tech-post-1" };
        public static Annotation Annotation2 = new Annotation() { AnnotationContent = "Annotation Content 2", EditorName = "JaneSmith", Id = "165e7c5b-c9db-405d-9f53-cef9b2f73d43", Slug = "tech-post-3" };
        public static Annotation Annotation3 = new Annotation() { AnnotationContent = "Annotation Content 3", EditorName = "LilithSmith", Id = "9f47620e-69a9-44d1-810b-3c54d16089b9", Slug = "blog-post-2" };
        public static Annotation Annotation4 = new Annotation() { AnnotationContent = "Annotation Content 4", EditorName = "V1", Id = "9d0861d7-b8b6-48e4-b8a9-72b8840256e4", Slug = "writing-post-1" };
        public static Annotation Annotation5 = new Annotation() { AnnotationContent = "Annotation Content 5", EditorName = "JohnUltrakill", Id = "599145e9-6da9-403b-979e-972bfbdc78f6", Slug = "writing-post-3" };
        public static Annotation Annotation6 = new Annotation() { AnnotationContent = "Annotation Content 6", EditorName = "LIKEANTENNATOHEAVEN", Id = "85d5bbce-c796-4e1b-894d-0d5bae843cbe", Slug = "brush-post-2" };

        */
    }
}
