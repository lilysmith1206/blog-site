using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Services;

namespace LylinkDb_UnitTests
{
    public class LylinkDb_PostCategoryUnitTests
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
        }
    }
}