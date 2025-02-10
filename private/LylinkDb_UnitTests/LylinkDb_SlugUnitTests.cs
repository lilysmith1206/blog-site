using LylinkBackend_DatabaseAccessLayer.Services;

namespace LylinkBackend_DatabaseAccessLayer_UnitTests
{
    public class LylinkDb_SlugUnitTests
    {
        [Fact]
        public void GetPostSlugs_ReturnsAllPostSlugs()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            ISlugRepository repository = new SlugRepository(context);

            IEnumerable<string> slugs = repository.GetPostSlugs();

            Assert.NotNull(slugs);

            IEnumerable<string> databaseSlugs = DatabaseUnitTestData.Posts.Select(post => post.Slug);

            foreach (string databaseSlug in databaseSlugs)
            {
                Assert.Contains(databaseSlug, slugs);
            }
        }

        [Fact]
        public void GetCategorySlugs_ReturnsAllCategorySlugs()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            ISlugRepository repository = new SlugRepository(context);

            IEnumerable<string> slugs = repository.GetCategorySlugs();

            Assert.NotNull(slugs);

            IEnumerable<string> databaseSlugs = DatabaseUnitTestData.Categories.Select(post => post.Slug);

            foreach (string databaseSlug in databaseSlugs)
            {
                Assert.Contains(databaseSlug, slugs);
            }
        }
    }
}
