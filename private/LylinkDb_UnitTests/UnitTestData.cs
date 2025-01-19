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
                    UseDateCreatedForSorting = false,
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
                return new Post {
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
                return new Post {
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
                return new Post {
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
                    UseDateCreatedForSorting = true,
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
                return new Post {
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
                return new Post {
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
                return new Post {
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
                    UseDateCreatedForSorting = true,
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
    }

    public static class UnitTestData
    {
        static UnitTestData()
        {
            IndexPost1.Parents = [KeyValuePair.Create(IndexCategory.Slug, IndexCategory.Name)];

            IndexCategory.ParentCategories = [];
            IndexCategory.ChildrenCategories = [KeyValuePair.Create(TechCategory.Slug, TechCategory.Name)];

            TechPost1.Parents = [KeyValuePair.Create(TechCategory.Slug, TechCategory.Name), KeyValuePair.Create(IndexCategory.Slug, IndexCategory.Name)];
            TechPost2.Parents = [KeyValuePair.Create(TechCategory.Slug, TechCategory.Name), KeyValuePair.Create(IndexCategory.Slug, IndexCategory.Name)];
            TechPost3.Parents = [KeyValuePair.Create(TechCategory.Slug, TechCategory.Name), KeyValuePair.Create(IndexCategory.Slug, IndexCategory.Name)];

            TechCategory.ChildrenCategories = [];
            TechCategory.Posts = [KeyValuePair.Create(TechPost1.Slug, TechPost1.Name), KeyValuePair.Create(TechPost2.Slug, TechPost2.Name), KeyValuePair.Create(TechPost3.Slug, TechPost3.Name)];

            MostRecentPostsPost1.Parents = [KeyValuePair.Create(IndexCategory.Slug, IndexCategory.Name)];
            MostRecentPostsPost2.Parents = [KeyValuePair.Create(IndexCategory.Slug, IndexCategory.Name)];
            MostRecentPostsPost3.Parents = [KeyValuePair.Create(IndexCategory.Slug, IndexCategory.Name)];

            MostRecentPostsCategory.ChildrenCategories = [];
            MostRecentPostsCategory.Posts = [KeyValuePair.Create(MostRecentPostsPost1.Slug, MostRecentPostsPost1.Name), KeyValuePair.Create(MostRecentPostsPost2.Slug, MostRecentPostsPost2.Name), KeyValuePair.Create(MostRecentPostsPost3.Slug, MostRecentPostsPost3.Name)];
        }

        public static PostPage IndexPost1 = new PostPage
        {
            Slug = "index-post-1",
            Title = "About Me",
            DateCreated = Dates.CurrentDateTime.AddDays(-10),
            DateModified = Dates.CurrentDateTime.AddDays(-5),
            Name = "Index Post 1",
            Keywords = "index, about me",
            Description = "About the blogger.",
            Body = "This post introduces the blogger on the index level category.",
            Parents = [KeyValuePair.Create(IndexCategory.Slug, IndexCategory.Name)],
            IsDraft = false
        };

        public static CategoryPage IndexCategory = new CategoryPage
        {
            Slug = "/",
            UseDateCreatedForSorting = false,
            Description = "Index category description",
            Keywords = "Index category keywords",
            Body = "index body",
            Name = "Index",
            Title = "Index Title",
            Posts = [KeyValuePair.Create(IndexPost1.Slug, IndexPost1.Name)],
        };

        public static PostPage TechPost1 = new PostPage
        {
            Slug = "tech-post-1",
            DateCreated = Dates.CurrentDateTime.AddDays(-10),
            DateModified = Dates.CurrentDateTime.AddDays(-5),
            Title = "Introduction to Gadgets",
            Name = "Tech Post 1",
            Keywords = "tech, gadgets, intro",
            Description = "An introductory guide to gadgets.",
            Body = "This post introduces the basics of gadgets and their use in modern tech.",
            Parents = [KeyValuePair.Create(TechCategory.Slug, TechCategory.Name), KeyValuePair.Create(IndexCategory.Slug, IndexCategory.Name)],
            IsDraft = false
        };

        public static PostPage TechPost2 = new PostPage
        {
            Slug = "tech-post-2",
            DateCreated = Dates.CurrentDateTime.AddDays(-20),
            DateModified = Dates.CurrentDateTime.AddDays(-10),
            Title = "Future of AI",
            Name = "Tech Post 2",
            Keywords = "AI, future, technology",
            Description = "Discussing the future of AI.",
            Body = "This post explores the possible advancements and impacts of AI in the future.",
            Parents = [KeyValuePair.Create(TechCategory.Slug, TechCategory.Name), KeyValuePair.Create(IndexCategory.Slug, IndexCategory.Name)],
            IsDraft = false
        };

        public static PostPage TechPost3 = new PostPage
        {
            Slug = "tech-post-3",
            DateCreated = Dates.CurrentDateTime.AddDays(-30),
            DateModified = Dates.CurrentDateTime.AddDays(-15),
            Title = "Top Tech Trends 2024",
            Name = "Tech Post 3",
            Keywords = "tech trends, 2024",
            Description = "The top technology trends to watch for in 2024.",
            Body = "This post covers the latest technology trends expected to shape the industry in 2024.",
            IsDraft = false
        };

        public static CategoryPage TechCategory = new CategoryPage
        {
            Slug = "tech",
            UseDateCreatedForSorting = true,
            Description = "Technology category description",
            Keywords = "tech, gadgets, computers",
            Body = "This is the body of the tech category.",
            Name = "Tech",
            Title = "Tech Title",
            ParentCategories = [KeyValuePair.Create(IndexCategory.Slug, IndexCategory.Name)]
        };

        public static PostPage MostRecentPostsPost1 = new PostPage
        {
            Slug = "most-recent-1",
            DateCreated = DateTime.MaxValue.AddYears(-1).AddDays(1),
            DateModified = DateTime.MaxValue.AddYears(-1).AddDays(1),
            Title = "Most Recent",
            Name = "Most Recent 1",
            Keywords = "most recent, 1",
            Description = "The most recent posts, 1.",
            Body = "This post is the least most recent one.",
            IsDraft = false
        };

        public static PostPage MostRecentPostsPost2 = new PostPage
        {
            Slug = "most-recent-2",
            DateCreated = DateTime.MaxValue.AddYears(-1).AddDays(2),
            DateModified = DateTime.MaxValue.AddYears(-1).AddDays(2),
            Title = "Most Recent",
            Name = "Most Recent 2",
            Keywords = "most recent, 2",
            Description = "The most recent posts, 3.",
            Body = "This post is the middle recent one.",
            IsDraft = false
        };

        public static PostPage MostRecentPostsPost3 = new PostPage
        {
            Slug = "most-recent-3",
            DateCreated = DateTime.MaxValue.AddYears(-1).AddDays(3),
            DateModified = DateTime.MaxValue.AddYears(-1).AddDays(3),
            Title = "Most Recent",
            Name = "Most Recent 3",
            Keywords = "most recent, 3",
            Description = "The most recent posts, 3.",
            Body = "This post is the most recent one.",
            IsDraft = false
        };

        public static CategoryPage MostRecentPostsCategory = new CategoryPage
        {
            Slug = "most-recent",
            UseDateCreatedForSorting = true,
            Description = "Technology category description",
            Keywords = "tech, gadgets, computers",
            Body = "This is the body of the tech category.",
            Name = "Tech",
            Title = "Tech Title",
            ParentCategories = [KeyValuePair.Create(IndexCategory.Slug, IndexCategory.Name)]
        };

        /*
        public static PostCategory IndexCategory = new PostCategory { CategoryId = 1, Slug = "", ParentId = null, UseDateCreatedForSorting = false, Description = "Index category description", Keywords = "Index category keywords", Body = "index body", CategoryName = "Index", Title = "Index Title" };
        public static PostCategory BlogCategory = new PostCategory { CategoryId = 3, Slug = "blog", ParentId = 1, UseDateCreatedForSorting = false, Description = "Blog category description", Keywords = "blog, articles, updates", Body = "This is the body of the blog category.", CategoryName = "Blog", Title = "Blog Title" };
        public static PostCategory WritingCategory = new PostCategory { CategoryId = 4, Slug = "writing", ParentId = 1, UseDateCreatedForSorting = true, Description = "Writing category description", Keywords = "writing, stories, fiction", Body = "This is the body of the writing category.", CategoryName = "Writing", Title = "Writing Title" };
        public static PostCategory BrushCategory = new PostCategory { CategoryId = 5, Slug = "brush", ParentId = 3, UseDateCreatedForSorting = false, Description = "Brush category description", Keywords = "brush, art, painting", Body = "This is the body of the brush category.", CategoryName = "Brush", Title = "Brush Title" };
        public static Post BlogPost1 = new Post { Slug = "blog-post-1", Title = "Starting Your Blog", ParentId = 3, DateCreated = Dates.CurrentDateTime.AddDays(-15), DateModified = Dates.CurrentDateTime.AddDays(-7), Name = "Blog Post 1", Keywords = "blog, start, guide", Description = "A guide to starting your own blog.", Body = "This post guides you through the steps to start your own blog and make it successful." };
        public static Post BlogPost2 = new Post { Slug = "blog-post-2", Title = "Content Ideas for Blogging", ParentId = 3, DateCreated = Dates.CurrentDateTime.AddDays(-25), DateModified = Dates.CurrentDateTime.AddDays(-12), Name = "Blog Post 2", Keywords = "blog, content, ideas", Description = "Generate great content ideas for your blog.", Body = "In this post, we share a variety of content ideas to keep your blog fresh and engaging." };
        public static Post BlogPost3 = new Post { Slug = "blog-post-3", Title = "SEO Tips for Bloggers", ParentId = 3, DateCreated = Dates.CurrentDateTime.AddDays(-35), DateModified = Dates.CurrentDateTime.AddDays(-20), Name = "Blog Post 3", Keywords = "SEO, blog, tips", Description = "SEO tips to help your blog rank higher.", Body = "This post provides SEO tips specifically aimed at bloggers to help them improve their search engine rankings." };
        public static Post WritingPost1 = new Post { Slug = "writing-post-1", Title = "Developing Characters", ParentId = 4, DateCreated = Dates.CurrentDateTime.AddDays(-12), DateModified = Dates.CurrentDateTime.AddDays(-8), Name = "Writing Post 1", Keywords = "writing, characters, fiction", Description = "How to develop engaging characters.", Body = "This post dives into strategies for developing memorable and engaging characters in your writing." };
        public static Post WritingPost2 = new Post { Slug = "writing-post-2", Title = "World-Building for Stories", ParentId = 4, DateCreated = Dates.CurrentDateTime.AddDays(-22), DateModified = Dates.CurrentDateTime.AddDays(-11), Name = "Writing Post 2", Keywords = "world-building, writing, fiction", Description = "Creating immersive worlds for your stories.", Body = "In this post, we explore the elements that go into creating immersive worlds for fictional stories." };
        public static Post WritingPost3 = new Post { Slug = "writing-post-3", Title = "Overcoming Writer’s Block", ParentId = 4, DateCreated = Dates.CurrentDateTime.AddDays(-32), DateModified = Dates.CurrentDateTime.AddDays(-18), Name = "Writing Post 3", Keywords = "writer's block, writing tips", Description = "Tips for overcoming writer’s block.", Body = "This post provides actionable tips for writers struggling with writer’s block." };
        public static Post BrushPost1 = new Post { Slug = "brush-post-1", Title = "Acrylic Painting Techniques", ParentId = 5, DateCreated = Dates.CurrentDateTime.AddDays(-18), DateModified = Dates.CurrentDateTime.AddDays(-10), Name = "Brush Post 1", Keywords = "acrylic painting, techniques", Description = "Techniques for working with acrylic paints.", Body = "This post explores a variety of acrylic painting techniques for beginners and experienced artists alike." };
        public static Post BrushPost2 = new Post { Slug = "brush-post-2", Title = "Choosing the Right Brushes", ParentId = 5, DateCreated = Dates.CurrentDateTime.AddDays(-28), DateModified = Dates.CurrentDateTime.AddDays(-15), Name = "Brush Post 2", Keywords = "art brushes, painting, tools", Description = "How to choose the right brushes for your painting style.", Body = "This post helps artists choose the best brushes for different painting techniques and styles." };
        public static Post BrushPost3 = new Post { Slug = "brush-post-3", Title = "Mixing Colors Effectively", ParentId = 5, DateCreated = Dates.CurrentDateTime.AddDays(-38), DateModified = Dates.CurrentDateTime.AddDays(-20), Name = "Brush Post 3", Keywords = "mixing colors, painting", Description = "Tips for mixing colors to achieve desired effects.", Body = "This post offers tips for artists looking to improve their color-mixing techniques." };
        public static Annotation Annotation1 = new Annotation() { AnnotationContent = "Annotation Content 1", EditorName = "JohnSmith", Id = "f3a5c3e2-166b-495a-827a-3314b82b130b", Slug = "tech-post-1" };
        public static Annotation Annotation2 = new Annotation() { AnnotationContent = "Annotation Content 2", EditorName = "JaneSmith", Id = "165e7c5b-c9db-405d-9f53-cef9b2f73d43", Slug = "tech-post-3" };
        public static Annotation Annotation3 = new Annotation() { AnnotationContent = "Annotation Content 3", EditorName = "LilithSmith", Id = "9f47620e-69a9-44d1-810b-3c54d16089b9", Slug = "blog-post-2" };
        public static Annotation Annotation4 = new Annotation() { AnnotationContent = "Annotation Content 4", EditorName = "V1", Id = "9d0861d7-b8b6-48e4-b8a9-72b8840256e4", Slug = "writing-post-1" };
        public static Annotation Annotation5 = new Annotation() { AnnotationContent = "Annotation Content 5", EditorName = "JohnUltrakill", Id = "599145e9-6da9-403b-979e-972bfbdc78f6", Slug = "writing-post-3" };
        public static Annotation Annotation6 = new Annotation() { AnnotationContent = "Annotation Content 6", EditorName = "LIKEANTENNATOHEAVEN", Id = "85d5bbce-c796-4e1b-894d-0d5bae843cbe", Slug = "brush-post-2" };

        */
    }
}
