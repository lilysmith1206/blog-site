using FluentMigrator;

namespace LylinkDatabase.MigrationScripts.Migrations._0;

[Migration(version: 0, transactionBehavior: TransactionBehavior.Default)]
public class CreateDatabase : Migration
{
    public override void Down()
    {
        Delete.DeletePosts();
        Delete.DeletePostHierarchies();
        Delete.DeleteAnnotationsTable();
    }

    public override void Up()
    {
        Create.CreateAnnotationsTable();
        Create.CreatePostHierarchies();
        Create.CreatePosts();
    }
}
