using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkBackend_DatabaseAccessLayer.Services;
using LylinkDb_UnitTests;
using System.Collections;

namespace LylinkBackend_DatabaseAccessLayer_UnitTests
{
    public class LylinkDb_PostUnitTests
    {
        [Fact]
        public void GetParentCategoriesFromParentId_ReturnsOnlyIndexWhenThatIsOnlyParent()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPostDatabaseService service = new DatabaseService(context);

            var result = service.GetParentCategoriesFromParentId(UnitTestData.IndexPost1.ParentId);

            var indexCategory = result.Single();

            Assert.Equal(indexCategory, UnitTestData.IndexCategory);
        }

        [Theory]
        [ClassData(typeof(PostParentCategoriesFromParentIdTestDataProvider))]
        public void GetParentCategoriesFromParentId_ReturnsAllParentCategories(int parentId, params PostCategory[] expectedResults)
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPostDatabaseService service = new DatabaseService(context);

            var result = service.GetParentCategoriesFromParentId(parentId);

            foreach ((var expectedCategory, var actualCategory) in expectedResults.Zip(result))
            {
                Assert.Equal(expectedCategory, actualCategory);
            }
        }
    }

    public class PostParentCategoriesFromParentIdTestDataProvider : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { UnitTestData.BlogPost1.ParentId ?? -1, UnitTestData.BlogCategory, UnitTestData.IndexCategory };
            yield return new object[] { UnitTestData.WritingPost1.ParentId ?? -1, UnitTestData.WritingCategory, UnitTestData.IndexCategory };
            yield return new object[] { UnitTestData.TechPost1.ParentId ?? -1, UnitTestData.TechCategory, UnitTestData.IndexCategory };
            yield return new object[] { UnitTestData.BrushPost1.ParentId ?? -1, UnitTestData.BrushCategory, UnitTestData.BlogCategory, UnitTestData.IndexCategory };
            yield return new object[] { UnitTestData.BrushPost1.ParentId ?? -1, UnitTestData.BrushCategory, UnitTestData.BlogCategory, UnitTestData.IndexCategory };
            yield return new object[] { UnitTestData.IndexPost1.ParentId ?? -1, UnitTestData.IndexCategory };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
