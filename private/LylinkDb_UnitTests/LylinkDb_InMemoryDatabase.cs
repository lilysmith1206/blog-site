using LylinkBackend_DatabaseAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace LylinkBackend_DatabaseAccessLayer_UnitTests
{
    public static class LylinkDb_InMemoryDatabase
    {
        public static LylinkdbContext GetFullDataInMemoryDatabase()
        {
            // Always create a new DbContext with a unique database name
            var context = GetInMemoryDbContext();

            // Reset database schema and populate with default data
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            FillInMemoryDatabaseWithUnitTestData(context);

            context.SaveChanges();

            return context;
        }

        private static void FillInMemoryDatabaseWithUnitTestData(LylinkdbContext context)
        {
            foreach (PostCategory category in DatabaseUnitTestData.Categories)
            {
                context.PostCategories.Add(category);
            }

            foreach (Post post in DatabaseUnitTestData.Posts)
            {
                context.Posts.Add(post);
            }

            foreach (Annotation annotation in DatabaseUnitTestData.Annotations)
            {
                context.Annotations.Add(annotation);
            }

            foreach (PostSortingMethod postSortingMethod in DatabaseUnitTestData.PostSortingMethods)
            {
                context.PostSortingMethods.Add(postSortingMethod);
            }
        }

        private static LylinkdbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<LylinkdbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Generate a unique database name per call
                .EnableSensitiveDataLogging()
                .Options;

            return new LylinkdbContext(options);
        }
    }
}
