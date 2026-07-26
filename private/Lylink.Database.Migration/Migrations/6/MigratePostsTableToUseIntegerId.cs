using FluentMigrator;

namespace Lylink.Database.Migration._6;

[Migration(version: 6)]
public class MigratePostsTableToUseIntegerId : FluentMigrator.Migration
{
    public override void Down()
    {
        Delete.DeleteTempPostsTable();
        Create.CreateTempPostsWithSlugAsId();
        Execute.CopyExistingPostDataToTempTable();
        Delete.DeletePostsTable();
        Execute.RenameTempPostsTableToPostsTable();
    }

    public override void Up()
    {
        Create.CreateTempPostsWithIntegerId();
        Execute.CopyExistingPostDataToTempTable();
        Delete.DeletePostsTable();
        Execute.RenameTempPostsTableToPostsTable();
    }
}
