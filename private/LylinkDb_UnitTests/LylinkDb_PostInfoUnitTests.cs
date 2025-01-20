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
            int parentId = UnitTestData.IndexCategoryInfo.Id;

            PostInfo newPostInfo = new PostInfo
            {
                Body = Body,
                Description = Description,
                IsDraft = IsDraft,
                Keywords = Keywords,
                Name = Name,
                ParentId = parentId,
                Slug = Slug,
                Title = Title
            };

            int newPostId = repository.CreatePost(newPostInfo);
            PostInfo? post = repository.GetPost(newPostId);

            Assert.NotNull(post);

            Assert.Equal(Slug, post.Slug);
            Assert.Equal(Title, post.Title);
            Assert.Equal(Name, post.Name);
            Assert.Equal(Keywords, post.Keywords);
            Assert.Equal(Description, post.Description);
            Assert.Equal(Body, post.Body);
            Assert.Equal(IsDraft, post.IsDraft);
            Assert.Equal(parentId, UnitTestData.IndexCategoryInfo.Id);
        }

        [Fact]
        public void CreatePost_GivenExistingPostSlug_ThrowsInvalidOperationException()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = repository.CreatePost(new PostInfo
                {
                    Slug = UnitTestData.IndexPostInfo1.Slug
                });
            });
        }

        [Fact]
        public void CreatePost_GivenExistingCategorySlug_ThrowsInvalidOperationException()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = repository.CreatePost(new PostInfo
                {
                    Slug = UnitTestData.IndexPostInfo1.Slug
                });
            });
        }

        [Fact]
        public void UpdatePost_UpdatesExistingPostPageData()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            PostInfo postInfo = new PostInfo
            {
                Slug = UnitTestData.IndexPostInfo1.Slug,
                Title = UnitTestData.IndexPostInfo1.Title + "A",
                Name = UnitTestData.IndexPostInfo1.Name + "A",
                Keywords = UnitTestData.IndexPostInfo1.Keywords + "A",
                Description = UnitTestData.IndexPostInfo1.Description + "A",
                Body = UnitTestData.IndexPostInfo1.Body + "A",
                IsDraft = UnitTestData.IndexPostInfo1.IsDraft == false,
                ParentId = DatabaseUnitTestData.IndexCategory.CategoryId
            };

            int updatedPostId = repository.UpdatePost(postInfo);
            PostInfo ? post = repository.GetPost(updatedPostId);

            Assert.NotNull(post);

            Assert.NotEqual(UnitTestData.IndexPostInfo1.Title, post.Title);
            Assert.NotEqual(UnitTestData.IndexPostInfo1.Name, post.Name);
            Assert.NotEqual(UnitTestData.IndexPostInfo1.Keywords, post.Keywords);
            Assert.NotEqual(UnitTestData.IndexPostInfo1.Description, post.Description);
            Assert.NotEqual(UnitTestData.IndexPostInfo1.Body, post.Body);
            Assert.NotEqual(UnitTestData.IndexPostInfo1.IsDraft, post.IsDraft);
        }

        [Fact]
        public void UpdatePost_GivenNewCategoryId_UpdatesPostToBeUnderNewCategory()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            PostInfo postInfo = new PostInfo
            {
                Slug = UnitTestData.IndexPostInfo1.Slug,
                Title = UnitTestData.IndexPostInfo1.Title,
                Name = UnitTestData.IndexPostInfo1.Name,
                Keywords = UnitTestData.IndexPostInfo1.Keywords,
                Description = UnitTestData.IndexPostInfo1.Description,
                Body = UnitTestData.IndexPostInfo1.Body,
                IsDraft = UnitTestData.IndexPostInfo1.IsDraft,
                ParentId = DatabaseUnitTestData.TechCategory.CategoryId
            };

            int updatedPostId = repository.UpdatePost(postInfo);
            PostInfo? post = repository.GetPost(updatedPostId);

            Assert.NotNull(post);

            Assert.NotEqual(UnitTestData.IndexPostInfo1.ParentId, post.ParentId);
        }

        [Fact]
        public void UpdatePost_GivenNonexistentPostSlug_ThrowsInvalidOperationException()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = repository.UpdatePost(new PostInfo
                {
                    Slug = "nonexistent post slug"
                });
            });
        }
    }
}
