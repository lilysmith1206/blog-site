using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Mappers;
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
            PageLink indexParentLink = new()
            {
                Description = DatabaseUnitTestData.IndexCategory.SlugNavigation.Description,
                Name = DatabaseUnitTestData.IndexCategory.SlugNavigation.Name,
                Slug = DatabaseUnitTestData.IndexCategory.Slug
            };

            PageLink techParentLink = new()
            {
                Description = DatabaseUnitTestData.TechCategory.SlugNavigation.Description,
                Name = DatabaseUnitTestData.TechCategory.SlugNavigation.Name,
                Slug = DatabaseUnitTestData.TechCategory.Slug
            };

            PageLink mostRecentPostsParentLink = new()
            {
                Description = DatabaseUnitTestData.MostRecentPostsCategory.SlugNavigation.Description,
                Name = DatabaseUnitTestData.MostRecentPostsCategory.SlugNavigation.Name,
                Slug = DatabaseUnitTestData.MostRecentPostsCategory.Slug
            };

            PageLink sortingPostsParentLink = new()
            {
                Description = DatabaseUnitTestData.PostSortingMethodCategory.SlugNavigation.Description,
                Name = DatabaseUnitTestData.PostSortingMethodCategory.SlugNavigation.Name,
                Slug = DatabaseUnitTestData.PostSortingMethodCategory.Slug
            };

            DatabaseUnitTestData.IndexPost1.Map([indexParentLink], out PostPage indexPage1);
            DatabaseUnitTestData.TechPost1.Map([indexParentLink, techParentLink], out PostPage techPage1);
            DatabaseUnitTestData.TechPost2.Map([indexParentLink, techParentLink], out PostPage techPage2);
            DatabaseUnitTestData.TechPost3.Map([indexParentLink, techParentLink], out PostPage techPage3);
            DatabaseUnitTestData.MostRecentPostsPost1.Map([indexParentLink, mostRecentPostsParentLink], out PostPage mostRecentPostsPage1);
            DatabaseUnitTestData.MostRecentPostsPost2.Map([indexParentLink, mostRecentPostsParentLink], out PostPage mostRecentPostsPage2);
            DatabaseUnitTestData.MostRecentPostsPost3.Map([indexParentLink, mostRecentPostsParentLink], out PostPage mostRecentPostsPage3);
            DatabaseUnitTestData.PostSortingMethodPost1.Map([indexParentLink, sortingPostsParentLink], out PostPage postSortingMethodPage1);
            DatabaseUnitTestData.PostSortingMethodPost2.Map([indexParentLink, sortingPostsParentLink], out PostPage postSortingMethodPage2);
            DatabaseUnitTestData.PostSortingMethodPost3.Map([indexParentLink, sortingPostsParentLink], out PostPage postSortingMethodPage3);
            DatabaseUnitTestData.PostSortingMethodPost4.Map([indexParentLink, sortingPostsParentLink], out PostPage postSortingMethodPage4);

            PostPage[] pages = [indexPage1, techPage1, techPage2, techPage3, mostRecentPostsPage1, mostRecentPostsPage2, mostRecentPostsPage3, postSortingMethodPage1, postSortingMethodPage2, postSortingMethodPage3, postSortingMethodPage4];

            foreach (PostPage page in pages)
            {
                yield return [page.Slug, page];
            }
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

            IEnumerable<PageLink> recentlyUpdatedPages = repository.GetRecentlyUpdatedPostInfos(count);

            foreach ((PostPage expected, PageLink actual) in expectedPostPages.Zip(recentlyUpdatedPages))
            {
                Assert.Equal(expected.Slug, actual.Slug);
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Description, actual.Description);
            }
        }

        public static IEnumerable<object[]> GetMostRecentPostsUnitTestData()
        {
            PageLink indexParentLink = new()
            {
                Description = DatabaseUnitTestData.IndexCategory.SlugNavigation.Description,
                Name = DatabaseUnitTestData.IndexCategory.SlugNavigation.Name,
                Slug = DatabaseUnitTestData.IndexCategory.Slug
            };

            PageLink mostRecentPostsParentLink = new()
            {
                Description = DatabaseUnitTestData.MostRecentPostsCategory.SlugNavigation.Description,
                Name = DatabaseUnitTestData.MostRecentPostsCategory.SlugNavigation.Name,
                Slug = DatabaseUnitTestData.MostRecentPostsCategory.Slug
            };

            DatabaseUnitTestData.MostRecentPostsPost1.Map([indexParentLink, mostRecentPostsParentLink], out PostPage mostRecentPostsPost1);
            DatabaseUnitTestData.MostRecentPostsPost2.Map([indexParentLink, mostRecentPostsParentLink], out PostPage mostRecentPostsPost2);
            DatabaseUnitTestData.MostRecentPostsPost3.Map([indexParentLink, mostRecentPostsParentLink], out PostPage mostRecentPostsPost3);

            yield return [1, mostRecentPostsPost3];
            yield return [2, mostRecentPostsPost3, mostRecentPostsPost2];
            yield return [3, mostRecentPostsPost3, mostRecentPostsPost2, mostRecentPostsPost1];
        }
    }
}
