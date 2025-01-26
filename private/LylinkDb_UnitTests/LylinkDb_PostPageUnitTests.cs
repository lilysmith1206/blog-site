using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Services;

namespace LylinkBackend_DatabaseAccessLayer_UnitTests
{
    public class LylinkDb_PostUnit0ests
    {

        [Theory]
        [MemberData(nameof(GetCategoryFromSlugUnitTestData))]
        public void GetCategoryFromSlug_ReturnsCategoryPage(string slug, PostPage expectedCategory)
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            PostPage? categoryPage = repository.GetPost(slug);

            Assert.NotNull(categoryPage);

            Assert.Equivalent(expectedCategory, categoryPage);
        }

        public static IEnumerable<object[]> GetCategoryFromSlugUnitTestData()
        {
            yield return [DatabaseUnitTestData.IndexPost1.Slug, UnitTestData.IndexPostPage1];
            yield return [DatabaseUnitTestData.TechPost1.Slug, UnitTestData.TechPostPage1];
            yield return [DatabaseUnitTestData.TechPost2.Slug, UnitTestData.TechPostPage2];
            yield return [DatabaseUnitTestData.TechPost3.Slug, UnitTestData.TechPostPage3];
            yield return [DatabaseUnitTestData.MostRecentPostsPost1.Slug, UnitTestData.MostRecentPostsPostPage1];
            yield return [DatabaseUnitTestData.MostRecentPostsPost2.Slug, UnitTestData.MostRecentPostsPostPage2];
            yield return [DatabaseUnitTestData.MostRecentPostsPost3.Slug, UnitTestData.MostRecentPostsPostPage3];
        }

        /// <summary>
        /// For this test to work, the DatabaseUnitTestData must have the most recent posts and most recent post category be given the highest date time values for date modified.
        /// If this is not the case, the test will fail.
        /// </summary>
        [Theory]
        [MemberData(nameof(GetMostRecentPostsUnitTestData))]
        public void GetMostRecentPosts_ReturnsMostRecentPosts(int count, params PostPage[] expectedPostPages)
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            IEnumerable<KeyValuePair<string, string>> recentlyUpdatedPages = repository.GetRecentlyUpdatedPostInfos(count);

            foreach ((PostPage expected, KeyValuePair<string, string> actual) in expectedPostPages.Zip(recentlyUpdatedPages))
            {
                Assert.Equal(expected.Slug, actual.Key);
                Assert.Equal(expected.Name, actual.Value);
            }
        }

        public static IEnumerable<object[]> GetMostRecentPostsUnitTestData()
        {
            yield return [1, UnitTestData.MostRecentPostsPostPage3];
            yield return [2, UnitTestData.MostRecentPostsPostPage3, UnitTestData.MostRecentPostsPostPage2];
            yield return [3, UnitTestData.MostRecentPostsPostPage3, UnitTestData.MostRecentPostsPostPage2, UnitTestData.MostRecentPostsPostPage1];
        }
    }
}
