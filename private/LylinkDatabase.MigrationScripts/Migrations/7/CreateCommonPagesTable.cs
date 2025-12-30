using FluentMigrator;

namespace LylinkDatabase.MigrationScripts.Migrations._7;

[Migration(version: 7)]
public class CreateCommonPagesTable : Migration
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
