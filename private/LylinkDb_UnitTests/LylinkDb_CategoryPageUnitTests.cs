using LylinkBackend_DatabaseAccessLayer.BusinessModels;
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
            yield return [DatabaseUnitTestData.IndexCategory.Slug, UnitTestData.IndexCategoryPage];
            yield return [DatabaseUnitTestData.TechCategory.Slug, UnitTestData.TechCategoryPage];
            yield return [DatabaseUnitTestData.MostRecentPostsCategory.Slug, UnitTestData.MostRecentPostsCategoryPage];
            yield return [DatabaseUnitTestData.PostSortingMethodCategory.Slug, UnitTestData.PostSortingMethodCategoryPage];
        }

        [Theory]
        [MemberData(nameof(GetCategoryPostSortingUnitTestData))]
        public void GetCategory_WithSortingMethod_SortsChildPostsAccordingly(PostSortingMethod sortingMethod, params KeyValuePair<string, string>[] expectedPostData)
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
                Assert.Equal(expected.Key, actual.Key);
                Assert.Equal(expected.Value, actual.Value);
            }
        }


        public static IEnumerable<object[]> GetCategoryPostSortingUnitTestData()
        {
            yield return [
                PostSortingMethod.ByDateCreatedAscending,
                KeyValuePair.Create(UnitTestData.PostSortingMethodPostPage4.Slug, UnitTestData.PostSortingMethodPostPage4.Name),
                KeyValuePair.Create(UnitTestData.PostSortingMethodPostPage1.Slug, UnitTestData.PostSortingMethodPostPage1.Name),
                KeyValuePair.Create(UnitTestData.PostSortingMethodPostPage2.Slug, UnitTestData.PostSortingMethodPostPage2.Name),
                KeyValuePair.Create(UnitTestData.PostSortingMethodPostPage3.Slug, UnitTestData.PostSortingMethodPostPage3.Name),
            ];

            yield return [
                PostSortingMethod.ByDateCreatedDescending,
                KeyValuePair.Create(UnitTestData.PostSortingMethodPostPage3.Slug, UnitTestData.PostSortingMethodPostPage3.Name),
                KeyValuePair.Create(UnitTestData.PostSortingMethodPostPage2.Slug, UnitTestData.PostSortingMethodPostPage2.Name),
                KeyValuePair.Create(UnitTestData.PostSortingMethodPostPage1.Slug, UnitTestData.PostSortingMethodPostPage1.Name),
                KeyValuePair.Create(UnitTestData.PostSortingMethodPostPage4.Slug, UnitTestData.PostSortingMethodPostPage4.Name),
            ];

            yield return [
                PostSortingMethod.ByDateModifiedAscending,
                KeyValuePair.Create(UnitTestData.PostSortingMethodPostPage3.Slug, UnitTestData.PostSortingMethodPostPage3.Name),
                KeyValuePair.Create(UnitTestData.PostSortingMethodPostPage4.Slug, UnitTestData.PostSortingMethodPostPage4.Name),
                KeyValuePair.Create(UnitTestData.PostSortingMethodPostPage1.Slug, UnitTestData.PostSortingMethodPostPage1.Name),
                KeyValuePair.Create(UnitTestData.PostSortingMethodPostPage2.Slug, UnitTestData.PostSortingMethodPostPage2.Name),
            ];

            yield return [
                PostSortingMethod.ByDateModifiedDescending,
                KeyValuePair.Create(UnitTestData.PostSortingMethodPostPage2.Slug, UnitTestData.PostSortingMethodPostPage2.Name),
                KeyValuePair.Create(UnitTestData.PostSortingMethodPostPage1.Slug, UnitTestData.PostSortingMethodPostPage1.Name),
                KeyValuePair.Create(UnitTestData.PostSortingMethodPostPage4.Slug, UnitTestData.PostSortingMethodPostPage4.Name),
                KeyValuePair.Create(UnitTestData.PostSortingMethodPostPage3.Slug, UnitTestData.PostSortingMethodPostPage3.Name),
            ];
        }
    }
}