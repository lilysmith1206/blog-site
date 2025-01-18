using LylinkBackend_DatabaseAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace LylinkDb_UnitTests
{
    public static class LylinkDb_InMemoryDatabase
    {
        private static object _lock = new();
        private static readonly LylinkdbContext _inMemoryDbContext;

        static LylinkDb_InMemoryDatabase()
        {
            _inMemoryDbContext = GetInMemoryDbContext();
        }

        public static LylinkdbContext GetFullDataInMemoryDatabase()
        {
            lock (_lock)
            {
                ClearInMemoryDatabase();

                _inMemoryDbContext.SaveChanges();

                FillInMemoryDatabaseWithUnitTestData();

                _inMemoryDbContext.SaveChanges();
            }

            return _inMemoryDbContext;
        }

        private static void ClearInMemoryDatabase()
        {
            if (_inMemoryDbContext.Posts.Any())
            {
                _inMemoryDbContext.Posts.RemoveRange(_inMemoryDbContext.Posts);
            }
            if (_inMemoryDbContext.PostCategories.Any())
            {
                _inMemoryDbContext.PostCategories.RemoveRange(_inMemoryDbContext.PostCategories);
            }
            if (_inMemoryDbContext.Annotations.Any())
            {
                _inMemoryDbContext.Annotations.RemoveRange(_inMemoryDbContext.Annotations);
            }
        }

        private static void FillInMemoryDatabaseWithUnitTestData()
        {
            foreach (PostCategory category in UnitTestData.Categories)
            {
                _inMemoryDbContext.PostCategories.Add(category);
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
