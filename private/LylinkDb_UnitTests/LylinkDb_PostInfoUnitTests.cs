using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Mappers;
using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkBackend_DatabaseAccessLayer.Services;

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
            foreach (Post post in DatabaseUnitTestData.Posts)
            {
                post.Map(out PostInfo postInfo);

                yield return [post.Id, postInfo];
            }
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
            int parentId = DatabaseUnitTestData.IndexCategory.CategoryId;

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
            Assert.Equal(parentId, DatabaseUnitTestData.IndexCategory.CategoryId);
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
                    Slug = DatabaseUnitTestData.IndexPost1.Slug
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
                    Slug = DatabaseUnitTestData.IndexPost1.Slug
                });
            });
        }

        [Fact]
        public void UpdatePost_UpdatesExistingPostPageData()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            string slug = DatabaseUnitTestData.IndexPost1.Slug;
            string title = DatabaseUnitTestData.IndexPost1.SlugNavigation.Title;
            string name = DatabaseUnitTestData.IndexPost1.SlugNavigation.Name;
            string keywords = DatabaseUnitTestData.IndexPost1.SlugNavigation.Keywords;
            string description = DatabaseUnitTestData.IndexPost1.SlugNavigation.Description;
            string body = DatabaseUnitTestData.IndexPost1.SlugNavigation.Body;
            bool isDraft = DatabaseUnitTestData.IndexPost1.IsDraft;

            PostInfo postInfo = new PostInfo
            {
                Slug = slug,
                Title = title + "A",
                Name = name + "A",
                Keywords = keywords + "A",
                Description = description + "A",
                Body = body + "A",
                IsDraft = isDraft == false,
                ParentId = DatabaseUnitTestData.IndexCategory.CategoryId
            };

            int updatedPostId = repository.UpdatePost(postInfo);
            PostInfo? post = repository.GetPost(updatedPostId);

            Assert.NotNull(post);

            Assert.NotEqual(title, post.Title);
            Assert.NotEqual(name, post.Name);
            Assert.NotEqual(keywords, post.Keywords);
            Assert.NotEqual(description, post.Description);
            Assert.NotEqual(body, post.Body);
            Assert.NotEqual(isDraft, post.IsDraft);
        }

        [Fact]
        public void UpdatePost_GivenNewCategoryId_UpdatesPostToBeUnderNewCategory()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            string slug = DatabaseUnitTestData.IndexPost1.Slug;
            string title = DatabaseUnitTestData.IndexPost1.SlugNavigation.Title;
            string name = DatabaseUnitTestData.IndexPost1.SlugNavigation.Name;
            string keywords = DatabaseUnitTestData.IndexPost1.SlugNavigation.Keywords;
            string description = DatabaseUnitTestData.IndexPost1.SlugNavigation.Description;
            string body = DatabaseUnitTestData.IndexPost1.SlugNavigation.Body;
            bool isDraft = DatabaseUnitTestData.IndexPost1.IsDraft;

            PostInfo postInfo = new PostInfo
            {
                Slug = slug,
                Title = title,
                Name = name,
                Keywords = keywords,
                Description = description,
                Body = body,
                IsDraft = isDraft,
                ParentId = DatabaseUnitTestData.TechCategory.CategoryId
            };

            int updatedPostId = repository.UpdatePost(postInfo);
            PostInfo? post = repository.GetPost(updatedPostId);

            Assert.NotNull(post);

            Assert.NotEqual(DatabaseUnitTestData.IndexPost1.ParentId, post.ParentId);
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
