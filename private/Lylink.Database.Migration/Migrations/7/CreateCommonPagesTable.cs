using FluentMigrator;

namespace Lylink.Database.Migration._7;

[Migration(version: 7)]
public class CreateCommonPagesTable : FluentMigrator.Migration
{
    public override void Down()
    {
        Create.CreatePageColumnsOnPost();
        Execute.CopyPostsPageDataFromPagesToPosts();
        Delete.DeleteSlugForeignKeyOnPostsToPages();

        Create.CreatePageColumnsOnPostCategories();
        Execute.CopyPostCategoriesPageDataFromPagesToPostCategories();
        Delete.DeleteSlugForeignKeyOnPostCategoriesToPages();

        Delete.DeletePagesTable();
    }

    public override void Up()
    {
        Create.CreatePagesTable();

        Execute.CopyPageDataForPostsToPages();
        Alter.CreateSlugForeignKeyOnPostsToPages();
        Delete.DropPageColumnsFromPosts();

        Execute.CopyPageDataForPostCategoriesToPages();
        Alter.CreateSlugForeignKeyOnPostCategoriesToPages();
        Delete.DropPageColumnsFromPostCategories();
    }
}
