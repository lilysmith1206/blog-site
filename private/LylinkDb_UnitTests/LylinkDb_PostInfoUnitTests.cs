using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Services;
using LylinkDb_UnitTests;

namespace LylinkBackend_DatabaseAccessLayer_UnitTests
{
    public class LylinkDb_PostInfoUnitTests
    {
        [Theory]
        [MemberData(nameof(GetPostFromIdUnitTestData))]
        public void GetPostFromId_ReturnsPostPage(int postId, PostInfo expectedCategory)
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            PostInfo? categoryPage = repository.GetPost(postId);

            Assert.NotNull(categoryPage);

            Assert.Equivalent(expectedCategory, categoryPage);
        }

        public static IEnumerable<object[]> GetPostFromIdUnitTestData()
        {
            yield return [DatabaseUnitTestData.IndexPost1.Id, UnitTestData.IndexPostInfo1];
            yield return [DatabaseUnitTestData.TechPost1.Id, UnitTestData.TechPostInfo1];
            yield return [DatabaseUnitTestData.TechPost2.Id, UnitTestData.TechPostInfo2];
            yield return [DatabaseUnitTestData.TechPost3.Id, UnitTestData.TechPostInfo3];
            yield return [DatabaseUnitTestData.MostRecentPostsPost1.Id, UnitTestData.MostRecentPostsPostInfo1];
            yield return [DatabaseUnitTestData.MostRecentPostsPost2.Id, UnitTestData.MostRecentPostsPostInfo2];
            yield return [DatabaseUnitTestData.MostRecentPostsPost3.Id, UnitTestData.MostRecentPostsPostInfo3];
        }

        [Fact]
        public void CreatePost_CreatesPost()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            const string Slug = "new-post";
            const string Title = "new post title";
            const string Name = "new post name";
            const string Keywords = "new post keywords";
            const string Description = "new post description";
            const string Body = "new post body";
            const bool IsDraft = false;
            int ParentId = UnitTestData.IndexCategoryInfo.Id;

            int newPostId = repository.CreatePost(Slug, Title, Name, Keywords, Description, Body, IsDraft, ParentId);

            PostInfo? post = repository.GetPost(newPostId);

            Assert.NotNull(post);

            Assert.Equal(Slug, post.Value.Slug);
            Assert.Equal(Title, post.Value.Title);
            Assert.Equal(Name, post.Value.Name);
            Assert.Equal(Keywords, post.Value.Keywords);
            Assert.Equal(Description, post.Value.Description);
            Assert.Equal(Body, post.Value.Body);
            Assert.Equal(IsDraft, post.Value.IsDraft);
            Assert.Equal(ParentId, UnitTestData.IndexCategoryInfo.Id);
        }

        [Fact]
        public void CreatePost_GivenExistingPostSlug_ThrowsInvalidOperationException()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = repository.CreatePost(UnitTestData.IndexPostInfo1.Slug, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, null);
            });
        }

        [Fact]
        public void CreatePost_GivenExistingCategorySlug_ThrowsInvalidOperationException()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = repository.CreatePost(UnitTestData.IndexCategoryInfo.Slug, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, null);
            });
        }

        [Fact]
        public void UpdatePost_UpdatesExistingPostPageData()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            int updatedPostId = repository.UpdatePost(
                UnitTestData.IndexPostInfo1.Slug,
                UnitTestData.IndexPostInfo1.Title + "A",
                UnitTestData.IndexPostInfo1.Name + "A",
                UnitTestData.IndexPostInfo1.Keywords + "A",
                UnitTestData.IndexPostInfo1.Description + "A",
                UnitTestData.IndexPostInfo1.Body + "A",
                UnitTestData.IndexPostInfo1.IsDraft == false,
                DatabaseUnitTestData.IndexCategory.CategoryId);

            PostInfo? post = repository.GetPost(updatedPostId);

            Assert.NotNull(post);

            Assert.NotEqual(UnitTestData.IndexPostInfo1.Title, post.Value.Title);
            Assert.NotEqual(UnitTestData.IndexPostInfo1.Name, post.Value.Name);
            Assert.NotEqual(UnitTestData.IndexPostInfo1.Keywords, post.Value.Keywords);
            Assert.NotEqual(UnitTestData.IndexPostInfo1.Description, post.Value.Description);
            Assert.NotEqual(UnitTestData.IndexPostInfo1.Body, post.Value.Body);
            Assert.NotEqual(UnitTestData.IndexPostInfo1.IsDraft, post.Value.IsDraft);
        }

        [Fact]
        public void UpdatePost_GivenNewCategoryId_UpdatesPostToBeUnderNewCategory()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            int updatedPostId = repository.UpdatePost(
                UnitTestData.IndexPostInfo1.Slug,
                UnitTestData.IndexPostInfo1.Title,
                UnitTestData.IndexPostInfo1.Name,
                UnitTestData.IndexPostInfo1.Keywords,
                UnitTestData.IndexPostInfo1.Description,
                UnitTestData.IndexPostInfo1.Body,
                UnitTestData.IndexPostInfo1.IsDraft,
                DatabaseUnitTestData.TechCategory.CategoryId);

            PostInfo? post = repository.GetPost(updatedPostId);

            Assert.NotNull(post);

            Assert.NotEqual(UnitTestData.IndexPostInfo1.ParentId, post.Value.ParentId);
        }

        [Fact]
        public void UpdatePost_GivenNonexistentPostSlug_ThrowsInvalidOperationException()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = repository.UpdatePost("nonexistent post slug", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, null);
            });
        }
    }
}
