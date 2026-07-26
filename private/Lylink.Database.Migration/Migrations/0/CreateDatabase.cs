using FluentMigrator;

namespace Lylink.Database.Migration._0;

[Migration(version: 0, transactionBehavior: TransactionBehavior.Default)]
public class CreateDatabase : FluentMigrator.Migration
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
