using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkBackend_DatabaseAccessLayer.Services;
using System.Collections;

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

        [Theory]
        [ClassData(typeof(PostCategoryParentCategoriesTestDataProvider))]
        public void GetParentCategoriesFromCategoryId_ReturnsOnlyIndexWhenThatIsOnlyParent(int categoryId, params PostCategory[] expectedResults)
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPostCategoryDatabaseService service = new DatabaseService(context);

            var result = service.GetParentCategoriesFromCategoryId(categoryId);

            foreach ((var expectedCategory, var actualCategory) in expectedResults.Zip(result))
            {
                Assert.Equal(expectedCategory, actualCategory);
            }
        }
    }

    public class PostCategoryParentCategoriesTestDataProvider : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { UnitTestData.IndexCategory.CategoryId, UnitTestData.IndexCategory };
            yield return new object[] { UnitTestData.BlogCategory.CategoryId, UnitTestData.IndexCategory };
            yield return new object[] { UnitTestData.WritingCategory.CategoryId, UnitTestData.IndexCategory };
            yield return new object[] { UnitTestData.TechCategory.CategoryId, UnitTestData.IndexCategory };
            yield return new object[] { UnitTestData.BrushCategory.CategoryId, UnitTestData.BlogCategory, UnitTestData.IndexCategory };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}