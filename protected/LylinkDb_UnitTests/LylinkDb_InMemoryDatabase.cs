using LylinkBackend_Database.Models;
using Microsoft.EntityFrameworkCore;

namespace LylinkDb_UnitTests
{
    public static class LylinkDb_InMemoryDatabase
    {
        private static readonly LylinkdbContext _inMemoryDbContext;

        static LylinkDb_InMemoryDatabase()
        {
            _inMemoryDbContext = GetInMemoryDbContext();
        }

        public static LylinkdbContext GetFullDataInMemoryDatabase()
        {
            _inMemoryDbContext.Posts.RemoveRange(_inMemoryDbContext.Posts);
            _inMemoryDbContext.PostHierarchies.RemoveRange(_inMemoryDbContext.PostHierarchies);

            _inMemoryDbContext.PostHierarchies.Add(new PostHierarchy { CategoryId = "f5f035f4-7ad9-411e-9d38-978c3df33964", Slug = "", ParentId = "", UseDateCreatedForSorting = false, Description = "Index category description", Keywords = "Index category keywords", Body = "index body", Name = "Index", Title = "Index Title" });
            _inMemoryDbContext.PostHierarchies.Add(new PostHierarchy { CategoryId = "a34b935e-e57c-4c40-803d-fb690fe90fcc", Slug = "tech", ParentId = "f5f035f4-7ad9-411e-9d38-978c3df33964", UseDateCreatedForSorting = true, Description = "Technology category description", Keywords = "tech, gadgets, computers", Body = "This is the body of the tech category.", Name = "Tech", Title = "Tech Title" });
            _inMemoryDbContext.PostHierarchies.Add(new PostHierarchy { CategoryId = "e14f7fee-f04c-47a5-a6d0-32650e87cb7d", Slug = "blog", ParentId = "f5f035f4-7ad9-411e-9d38-978c3df33964", UseDateCreatedForSorting = false, Description = "Blog category description", Keywords = "blog, articles, updates", Body = "This is the body of the blog category.", Name = "Blog", Title = "Blog Title" });
            _inMemoryDbContext.PostHierarchies.Add(new PostHierarchy { CategoryId = "c088f8aa-e94d-4b02-85e7-baf5504c693f", Slug = "writing", ParentId = "f5f035f4-7ad9-411e-9d38-978c3df33964", UseDateCreatedForSorting = true, Description = "Writing category description", Keywords = "writing, stories, fiction", Body = "This is the body of the writing category.", Name = "Writing", Title = "Writing Title" });
            _inMemoryDbContext.PostHierarchies.Add(new PostHierarchy { CategoryId = "0a1e0b05-f98b-4fc6-aa3f-f61af3bb2b70", Slug = "brush", ParentId = "e14f7fee-f04c-47a5-a6d0-32650e87cb7d", UseDateCreatedForSorting = false, Description = "Brush category description", Keywords = "brush, art, painting", Body = "This is the body of the brush category.", Name = "Brush", Title = "Brush Title" });
            _inMemoryDbContext.Posts.Add(new Post { Slug = "tech-post-1", Title = "Introduction to Gadgets", ParentId = "a34b935e-e57c-4c40-803d-fb690fe90fcc", DateCreated = DateTime.Now.AddDays(-10), DateModified = DateTime.Now.AddDays(-5), Name = "Tech Post 1", Keywords = "tech, gadgets, intro", Description = "An introductory guide to gadgets.", Body = "This post introduces the basics of gadgets and their use in modern tech." });
            _inMemoryDbContext.Posts.Add(new Post { Slug = "tech-post-2", Title = "Future of AI", ParentId = "a34b935e-e57c-4c40-803d-fb690fe90fcc", DateCreated = DateTime.Now.AddDays(-20), DateModified = DateTime.Now.AddDays(-10), Name = "Tech Post 2", Keywords = "AI, future, technology", Description = "Discussing the future of AI.", Body = "This post explores the possible advancements and impacts of AI in the future." });
            _inMemoryDbContext.Posts.Add(new Post { Slug = "tech-post-3", Title = "Top Tech Trends 2024", ParentId = "a34b935e-e57c-4c40-803d-fb690fe90fcc", DateCreated = DateTime.Now.AddDays(-30), DateModified = DateTime.Now.AddDays(-15), Name = "Tech Post 3", Keywords = "tech trends, 2024", Description = "The top technology trends to watch for in 2024.", Body = "This post covers the latest technology trends expected to shape the industry in 2024." });
            _inMemoryDbContext.Posts.Add(new Post { Slug = "blog-post-1", Title = "Starting Your Blog", ParentId = "e14f7fee-f04c-47a5-a6d0-32650e87cb7d", DateCreated = DateTime.Now.AddDays(-15), DateModified = DateTime.Now.AddDays(-7), Name = "Blog Post 1", Keywords = "blog, start, guide", Description = "A guide to starting your own blog.", Body = "This post guides you through the steps to start your own blog and make it successful." });
            _inMemoryDbContext.Posts.Add(new Post { Slug = "blog-post-2", Title = "Content Ideas for Blogging", ParentId = "e14f7fee-f04c-47a5-a6d0-32650e87cb7d", DateCreated = DateTime.Now.AddDays(-25), DateModified = DateTime.Now.AddDays(-12), Name = "Blog Post 2", Keywords = "blog, content, ideas", Description = "Generate great content ideas for your blog.", Body = "In this post, we share a variety of content ideas to keep your blog fresh and engaging." });
            _inMemoryDbContext.Posts.Add(new Post { Slug = "blog-post-3", Title = "SEO Tips for Bloggers", ParentId = "e14f7fee-f04c-47a5-a6d0-32650e87cb7d", DateCreated = DateTime.Now.AddDays(-35), DateModified = DateTime.Now.AddDays(-20), Name = "Blog Post 3", Keywords = "SEO, blog, tips", Description = "SEO tips to help your blog rank higher.", Body = "This post provides SEO tips specifically aimed at bloggers to help them improve their search engine rankings." });
            _inMemoryDbContext.Posts.Add(new Post { Slug = "writing-post-1", Title = "Developing Characters", ParentId = "c088f8aa-e94d-4b02-85e7-baf5504c693f", DateCreated = DateTime.Now.AddDays(-12), DateModified = DateTime.Now.AddDays(-8), Name = "Writing Post 1", Keywords = "writing, characters, fiction", Description = "How to develop engaging characters.", Body = "This post dives into strategies for developing memorable and engaging characters in your writing." });
            _inMemoryDbContext.Posts.Add(new Post { Slug = "writing-post-2", Title = "World-Building for Stories", ParentId = "c088f8aa-e94d-4b02-85e7-baf5504c693f", DateCreated = DateTime.Now.AddDays(-22), DateModified = DateTime.Now.AddDays(-11), Name = "Writing Post 2", Keywords = "world-building, writing, fiction", Description = "Creating immersive worlds for your stories.", Body = "In this post, we explore the elements that go into creating immersive worlds for fictional stories." });
            _inMemoryDbContext.Posts.Add(new Post { Slug = "writing-post-3", Title = "Overcoming Writer’s Block", ParentId = "c088f8aa-e94d-4b02-85e7-baf5504c693f", DateCreated = DateTime.Now.AddDays(-32), DateModified = DateTime.Now.AddDays(-18), Name = "Writing Post 3", Keywords = "writer's block, writing tips", Description = "Tips for overcoming writer’s block.", Body = "This post provides actionable tips for writers struggling with writer’s block." });
            _inMemoryDbContext.Posts.Add(new Post { Slug = "brush-post-1", Title = "Acrylic Painting Techniques", ParentId = "0a1e0b05-f98b-4fc6-aa3f-f61af3bb2b70", DateCreated = DateTime.Now.AddDays(-18), DateModified = DateTime.Now.AddDays(-10), Name = "Brush Post 1", Keywords = "acrylic painting, techniques", Description = "Techniques for working with acrylic paints.", Body = "This post explores a variety of acrylic painting techniques for beginners and experienced artists alike." });
            _inMemoryDbContext.Posts.Add(new Post { Slug = "brush-post-2", Title = "Choosing the Right Brushes", ParentId = "0a1e0b05-f98b-4fc6-aa3f-f61af3bb2b70", DateCreated = DateTime.Now.AddDays(-28), DateModified = DateTime.Now.AddDays(-15), Name = "Brush Post 2", Keywords = "art brushes, painting, tools", Description = "How to choose the right brushes for your painting style.", Body = "This post helps artists choose the best brushes for different painting techniques and styles." });
            _inMemoryDbContext.Posts.Add(new Post { Slug = "brush-post-3", Title = "Mixing Colors Effectively", ParentId = "0a1e0b05-f98b-4fc6-aa3f-f61af3bb2b70", DateCreated = DateTime.Now.AddDays(-38), DateModified = DateTime.Now.AddDays(-20), Name = "Brush Post 3", Keywords = "mixing colors, painting", Description = "Tips for mixing colors to achieve desired effects.", Body = "This post offers tips for artists looking to improve their color-mixing techniques." });

            _inMemoryDbContext.SaveChanges();

            return _inMemoryDbContext;
        }

        private static LylinkdbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<LylinkdbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            return new LylinkdbContext(options);
        }
    }
}
