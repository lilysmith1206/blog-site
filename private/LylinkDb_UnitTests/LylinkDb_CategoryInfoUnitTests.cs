using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Services;
using LylinkDb_UnitTests;

namespace LylinkBackend_DatabaseAccessLayer_UnitTests
{
    public class LylinkDb_CategoryInfoUnitTests
    {
        [Theory]
        [MemberData(nameof(GetCategoryFromIdUnitTestData))]
        public void GetCategoryFromId_ReturnsCategoryPage(int categoryId, CategoryInfo expectedCategory)
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            CategoryInfo? categoryPage = repository.GetCategory(categoryId);

            Assert.NotNull(categoryPage);

            Assert.Equivalent(expectedCategory, categoryPage);
        }

        public static IEnumerable<object[]> GetCategoryFromIdUnitTestData()
        {
            yield return [DatabaseUnitTestData.IndexCategory.CategoryId, UnitTestData.IndexCategoryInfo];
            yield return [DatabaseUnitTestData.TechCategory.CategoryId, UnitTestData.TechCategoryInfo];
            yield return [DatabaseUnitTestData.MostRecentPostsCategory.CategoryId, UnitTestData.MostRecentPostsCategoryInfo];
        }

        [Fact]
        public void CreateCategory_CreatesCategory()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            const string Slug = "new-post";
            const string Title = "new post title";
            const string Name = "new post name";
            const string Keywords = "new post keywords";
            const string Description = "new post description";
            const string Body = "new post body";
            const bool isSortedByDateCreated = false;
            int parentId = UnitTestData.IndexCategoryInfo.Id;

            int newPostId = repository.CreateCategory(Slug, Title, Name, Keywords, Description, Body, isSortedByDateCreated, parentId);

            CategoryInfo? post = repository.GetCategory(newPostId);

            Assert.NotNull(post);

            Assert.Equal(Slug, post.Value.Slug);
            Assert.Equal(Title, post.Value.Title);
            Assert.Equal(Name, post.Value.Name);
            Assert.Equal(Keywords, post.Value.Keywords);
            Assert.Equal(Description, post.Value.Description);
            Assert.Equal(Body, post.Value.Body);
            Assert.Equal(isSortedByDateCreated, post.Value.UseDateCreatedForSorting);
            Assert.Equal(parentId, UnitTestData.IndexCategoryInfo.Id);
        }

        [Fact]
        public void CreateCategory_GivenExistingPostSlug_ThrowsInvalidOperationException()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = repository.CreateCategory(UnitTestData.IndexPostPage1.Slug, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, null);
            });
        }

        [Fact]
        public void CreateCategory_GivenExistingCategorySlug_ThrowsInvalidOperationException()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = repository.CreateCategory(UnitTestData.IndexCategoryPage.Slug, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, null);
            });
        }

        [Fact]
        public void UpdateCategory_UpdatesExistingCategoryPageData()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            int updatedCategoryId = repository.UpdateCategory(
                UnitTestData.IndexCategoryInfo.Slug,
                UnitTestData.IndexCategoryInfo.Title + "A",
                UnitTestData.IndexCategoryInfo.Name + "A",
                UnitTestData.IndexCategoryInfo.Keywords + "A",
                UnitTestData.IndexCategoryInfo.Description + "A",
                UnitTestData.IndexCategoryInfo.Body + "A",
                UnitTestData.IndexCategoryInfo.UseDateCreatedForSorting == false,
                null);

            CategoryInfo? post = repository.GetCategory(updatedCategoryId);

            Assert.NotNull(post);

            Assert.NotEqual(UnitTestData.IndexCategoryInfo.Title, post.Value.Title);
            Assert.NotEqual(UnitTestData.IndexCategoryInfo.Name, post.Value.Name);
            Assert.NotEqual(UnitTestData.IndexCategoryInfo.Keywords, post.Value.Keywords);
            Assert.NotEqual(UnitTestData.IndexCategoryInfo.Description, post.Value.Description);
            Assert.NotEqual(UnitTestData.IndexCategoryInfo.Body, post.Value.Body);
            Assert.NotEqual(UnitTestData.IndexCategoryInfo.UseDateCreatedForSorting, post.Value.UseDateCreatedForSorting);
        }

        [Fact]
        public void UpdateCategory_GivenNewCategoryId_UpdatesCategoryToBeUnderNewCategory()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            int updatedPostId = repository.UpdateCategory(
                UnitTestData.MostRecentPostsCategoryInfo.Slug,
                UnitTestData.MostRecentPostsCategoryInfo.Title,
                UnitTestData.MostRecentPostsCategoryInfo.Name,
                UnitTestData.MostRecentPostsCategoryInfo.Keywords,
                UnitTestData.MostRecentPostsCategoryInfo.Description,
                UnitTestData.MostRecentPostsCategoryInfo.Body,
                UnitTestData.MostRecentPostsCategoryInfo.UseDateCreatedForSorting,
                DatabaseUnitTestData.TechCategory.CategoryId);

            CategoryInfo? category = repository.GetCategory(updatedPostId);

            Assert.NotNull(category);

            Assert.Equal(UnitTestData.TechCategoryInfo.Id, category.Value.ParentId);
        }

        [Fact]
        public void UpdatePost_GivenNonexistentPostSlug_ThrowsInvalidOperationException()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = repository.UpdateCategory("nonexistent post slug", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, null);
            });
        }
    }
}
