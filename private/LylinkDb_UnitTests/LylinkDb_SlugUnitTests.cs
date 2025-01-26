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

            Assert.Contains(UnitTestData.IndexPostPage1.Slug, slugs);
            Assert.Contains(UnitTestData.TechPostPage1.Slug, slugs);
            Assert.Contains(UnitTestData.TechPostPage2.Slug, slugs);
            Assert.Contains(UnitTestData.TechPostPage3.Slug, slugs);
            Assert.Contains(UnitTestData.MostRecentPostsPostPage1.Slug, slugs);
            Assert.Contains(UnitTestData.MostRecentPostsPostPage2.Slug, slugs);
            Assert.Contains(UnitTestData.MostRecentPostsPostPage3.Slug, slugs);
        }

        [Fact]
        public void GetCategorySlugs_ReturnsAllCategorySlugs()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            ISlugRepository repository = new SlugRepository(context);

            IEnumerable<string> slugs = repository.GetCategorySlugs();

            Assert.NotNull(slugs);

            Assert.Contains(UnitTestData.IndexCategoryPage.Slug, slugs);
            Assert.Contains(UnitTestData.TechCategoryPage.Slug, slugs);
            Assert.Contains(UnitTestData.MostRecentPostsCategoryPage.Slug, slugs);
        }
    }
}
