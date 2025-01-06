using LylinkBackend_DatabaseAccessLayer.Services;

namespace LylinkDb_UnitTests
{
    public class LylinkDb_PostCategoryUnitTests
    {
        [Fact]
        public void GetAllCategorySlugs_ShouldReturnCategorySlugs()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPostCategoryDatabaseService service = new DatabaseService(context);

            var result = service.GetAllCategorySlugs();

            Assert.Equal(UnitTestData.Categories.Count, result.Count());

            Assert.Contains(UnitTestData.TechCategory.Slug, result);
            Assert.Contains(UnitTestData.BlogCategory.Slug, result);
            Assert.Contains(UnitTestData.IndexCategory.Slug, result);
            Assert.Contains(UnitTestData.WritingCategory.Slug, result);
            Assert.Contains(UnitTestData.BrushCategory.Slug, result);
        }

        [Fact]
        public void GetChildCategoriesOfCategory_ShouldReturnChildCategories()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPostCategoryDatabaseService service = new DatabaseService(context);

            var result = service.GetChildCategoriesOfCategory(UnitTestData.BlogCategory.CategoryId);

            Assert.NotNull(result);
            Assert.Single(result);

            var brushCategory = result.Single();

            Assert.Equal(UnitTestData.BrushCategory.CategoryId, brushCategory.CategoryId);
            Assert.Equal(UnitTestData.BrushCategory.Slug, brushCategory.Slug);
            Assert.Equal(UnitTestData.BrushCategory.ParentId, brushCategory.ParentId);
            Assert.Equal(UnitTestData.BrushCategory.UseDateCreatedForSorting, brushCategory.UseDateCreatedForSorting);
            Assert.Equal(UnitTestData.BrushCategory.Description, brushCategory.Description);
            Assert.Equal(UnitTestData.BrushCategory.Keywords, brushCategory.Keywords);
            Assert.Equal(UnitTestData.BrushCategory.Body, brushCategory.Body);
            Assert.Equal(UnitTestData.BrushCategory.CategoryName, brushCategory.CategoryName);
            Assert.Equal(UnitTestData.BrushCategory.Title, brushCategory.Title);
        }
    }
}