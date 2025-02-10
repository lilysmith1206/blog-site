using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Mappers;
using LylinkBackend_DatabaseAccessLayer.Services;

namespace LylinkBackend_DatabaseAccessLayer_UnitTests
{
    public class LylinkDb_CategoryPageUnitTests
    {
        [Theory]
        [MemberData(nameof(GetCategoryFromSlugUnitTestData))]
        public void GetCategoryFromSlug_ReturnsCategoryPage(string slug, CategoryPage expectedCategory)
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            CategoryPage? categoryPage = repository.GetCategory(slug);

            Assert.NotNull(categoryPage);

            Assert.Equivalent(expectedCategory, categoryPage);
        }

        public static IEnumerable<object[]> GetCategoryFromSlugUnitTestData()
        {
            PageLink indexLink = DatabaseUnitTestData.IndexCategory.Map();
            PageLink techLink = DatabaseUnitTestData.TechCategory.Map();
            PageLink mostRecentPostsLink = DatabaseUnitTestData.MostRecentPostsCategory.Map();
            PageLink postSortingMethodsLink = DatabaseUnitTestData.PostSortingMethodCategory.Map();

            PageLink indexPost1Link = DatabaseUnitTestData.IndexPost1.Map();
            PageLink techPost1Link = DatabaseUnitTestData.TechPost1.Map();
            PageLink techPost2Link = DatabaseUnitTestData.TechPost2.Map();
            PageLink techPost3Link = DatabaseUnitTestData.TechPost3.Map();
            PageLink mostRecentPost1Link = DatabaseUnitTestData.MostRecentPostsPost1.Map();
            PageLink mostRecentPost2Link = DatabaseUnitTestData.MostRecentPostsPost2.Map();
            PageLink mostRecentPost3Link = DatabaseUnitTestData.MostRecentPostsPost3.Map();
            PageLink postSortingMethodsPost1Link = DatabaseUnitTestData.PostSortingMethodPost1.Map();
            PageLink postSortingMethodsPost2Link = DatabaseUnitTestData.PostSortingMethodPost2.Map();
            PageLink postSortingMethodsPost3Link = DatabaseUnitTestData.PostSortingMethodPost3.Map();
            PageLink postSortingMethodsPost4Link = DatabaseUnitTestData.PostSortingMethodPost4.Map();

            DatabaseUnitTestData.IndexCategory.Map(
                [indexPost1Link],
                [indexLink],
                [techLink, mostRecentPostsLink, postSortingMethodsLink],
                (PostSortingMethod)(DatabaseUnitTestData.IndexCategory.PostSortingMethodId ?? 1),
                out CategoryPage indexCategoryPage
            );
            
            DatabaseUnitTestData.TechCategory.Map(
                [techPost1Link, techPost2Link, techPost3Link],
                [indexLink],
                [],
                (PostSortingMethod)(DatabaseUnitTestData.TechCategory.PostSortingMethodId ?? 1),
                out CategoryPage techCategoryPage);

            DatabaseUnitTestData.MostRecentPostsCategory.Map(
                [mostRecentPost1Link, mostRecentPost2Link, mostRecentPost3Link],
                [indexLink],
                [],
                (PostSortingMethod)(DatabaseUnitTestData.MostRecentPostsCategory.PostSortingMethodId ?? 1),
                out CategoryPage mostRecentPostsCategoryPage);

            DatabaseUnitTestData.PostSortingMethodCategory.Map(
                [postSortingMethodsPost1Link, postSortingMethodsPost2Link, postSortingMethodsPost3Link, postSortingMethodsPost4Link],
                [indexLink],
                [],
                (PostSortingMethod)(DatabaseUnitTestData.PostSortingMethodCategory.PostSortingMethodId ?? 1),
                out CategoryPage postSortingMethodCategoryPage);

            yield return [DatabaseUnitTestData.IndexCategory.Slug, indexCategoryPage];
            yield return [DatabaseUnitTestData.TechCategory.Slug, techCategoryPage];
            yield return [DatabaseUnitTestData.MostRecentPostsCategory.Slug, mostRecentPostsCategoryPage];
            yield return [DatabaseUnitTestData.PostSortingMethodCategory.Slug, postSortingMethodCategoryPage];
        }

        [Theory]
        [MemberData(nameof(GetCategoryPostSortingUnitTestData))]
        public void GetCategory_WithSortingMethod_SortsChildPostsAccordingly(PostSortingMethod sortingMethod, params PageLink[] expectedPostData)
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository pageManagementRepository = new PageManagementRepository(context);
            IPageRepository repository = new PageRepository(context);

            CategoryInfo categoryInfo = pageManagementRepository.GetCategory(DatabaseUnitTestData.PostSortingMethodCategory.CategoryId);

            categoryInfo.PostSortingMethod = sortingMethod;

            _ = pageManagementRepository.UpdateCategory(categoryInfo);

            CategoryPage? categoryPage = repository.GetCategory(categoryInfo.Slug);

            Assert.NotNull(categoryPage);
            Assert.NotNull(categoryPage.Value.Posts);

            Assert.Equal(expectedPostData.Length, categoryPage.Value.Posts.Count());

            foreach ((var expected, var actual) in expectedPostData.Zip(categoryPage.Value.Posts))
            {
                Assert.Equal(expected.Description, actual.Description);
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Slug, actual.Slug);
            }
        }

        public static IEnumerable<object[]> GetCategoryPostSortingUnitTestData()
        {
            PageLink sortingPage1Link = new()
            {
                Description = DatabaseUnitTestData.PostSortingMethodPost1.SlugNavigation.Description,
                Name = DatabaseUnitTestData.PostSortingMethodPost1.SlugNavigation.Name,
                Slug = DatabaseUnitTestData.PostSortingMethodPost1.SlugNavigation.Slug,
            };

            PageLink sortingPage2Link = new()
            {
                Description = DatabaseUnitTestData.PostSortingMethodPost2.SlugNavigation.Description,
                Name = DatabaseUnitTestData.PostSortingMethodPost2.SlugNavigation.Name,
                Slug = DatabaseUnitTestData.PostSortingMethodPost2.SlugNavigation.Slug,
            };
            
            PageLink sortingPage3Link = new()
            {
                Description = DatabaseUnitTestData.PostSortingMethodPost3.SlugNavigation.Description,
                Name = DatabaseUnitTestData.PostSortingMethodPost3.SlugNavigation.Name,
                Slug = DatabaseUnitTestData.PostSortingMethodPost3.SlugNavigation.Slug,
            };
            
            PageLink sortingPage4Link = new()
            {
                Description = DatabaseUnitTestData.PostSortingMethodPost4.SlugNavigation.Description,
                Name = DatabaseUnitTestData.PostSortingMethodPost4.SlugNavigation.Name,
                Slug = DatabaseUnitTestData.PostSortingMethodPost4.SlugNavigation.Slug,
            };

            yield return [
                PostSortingMethod.ByDateCreatedAscending,
                sortingPage4Link,
                sortingPage1Link,
                sortingPage2Link,
                sortingPage3Link,
            ];

            yield return [
                PostSortingMethod.ByDateCreatedDescending,
                sortingPage3Link,
                sortingPage2Link,
                sortingPage1Link,
                sortingPage4Link,
            ];

            yield return [
                PostSortingMethod.ByDateModifiedAscending,
                sortingPage3Link,
                sortingPage4Link,
                sortingPage1Link,
                sortingPage2Link,
            ];

            yield return [
                PostSortingMethod.ByDateModifiedDescending,
                sortingPage2Link,
                sortingPage1Link,
                sortingPage4Link,
                sortingPage3Link,
            ];
        }
    }
}