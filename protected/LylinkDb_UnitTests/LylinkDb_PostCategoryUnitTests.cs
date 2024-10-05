using LylinkBackend_Database.Services;
using System.Xml.Linq;

namespace LylinkDb_UnitTests
{
    public class LylinkDb_PostCategoryUnitTests
    {
        [Fact]
        public void GetAllCategorySlugs_ShouldReturnCategorySlugs()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            var service = new DatabaseService(context);

            var result = service.GetAllCategorySlugs();

            Assert.Equal(5, result.Count());
            Assert.Contains("tech", result);
            Assert.Contains("blog", result);
            Assert.Contains("", result);
            Assert.Contains("writing", result);
            Assert.Contains("brush", result);
        }

        [Fact]
        public void GetChildCategoriesOfCategory_ShouldReturnChildCategories()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            var service = new DatabaseService(context);

            var result = service.GetChildCategoriesOfCategory("e14f7fee-f04c-47a5-a6d0-32650e87cb7d");

            Assert.NotNull(result);
            Assert.Single(result);

            var brushCategory = result.Single();

            Assert.Equal("0a1e0b05-f98b-4fc6-aa3f-f61af3bb2b70", brushCategory.CategoryId);
            Assert.Equal("brush", brushCategory.Slug);
            Assert.Equal("e14f7fee-f04c-47a5-a6d0-32650e87cb7d", brushCategory.ParentId);
            Assert.Equal(false, brushCategory.UseDateCreatedForSorting);
            Assert.Equal("Brush category description", brushCategory.Description);
            Assert.Equal("brush, art, painting", brushCategory.Keywords);
            Assert.Equal("This is the body of the brush category.", brushCategory.Body);
            Assert.Equal("Brush", brushCategory.Name);
            Assert.Equal("Brush Title", brushCategory.Title);
        }
    }
}