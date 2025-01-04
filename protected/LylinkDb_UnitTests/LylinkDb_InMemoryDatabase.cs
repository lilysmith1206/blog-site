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
            ClearInMemoryDatabase();

            _inMemoryDbContext.SaveChanges();

            FillInMemoryDatabaseWithUnitTestData();

            _inMemoryDbContext.SaveChanges();

            return _inMemoryDbContext;
        }

        private static void ClearInMemoryDatabase()
        {
            if (_inMemoryDbContext.Posts.Any())
            {
                _inMemoryDbContext.Posts.RemoveRange(_inMemoryDbContext.Posts);
            }
            if (_inMemoryDbContext.PostHierarchies.Any())
            {
                _inMemoryDbContext.PostHierarchies.RemoveRange(_inMemoryDbContext.PostHierarchies);
            }
            if (_inMemoryDbContext.Annotations.Any())
            {
                _inMemoryDbContext.Annotations.RemoveRange(_inMemoryDbContext.Annotations);
            }
        }

        private static void FillInMemoryDatabaseWithUnitTestData()
        {
            foreach (PostHierarchy category in UnitTestData.Categories)
            {
                _inMemoryDbContext.PostHierarchies.Add(category);
            }

            foreach (Post post in UnitTestData.Posts)
            {
                _inMemoryDbContext.Posts.Add(post);
            }

            foreach (Annotation annotation in UnitTestData.Annotations)
            {
                _inMemoryDbContext.Annotations.Add(annotation);
            }
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
