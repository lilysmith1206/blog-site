using FluentMigrator;

namespace LylinkDatabase.MigrationScripts.Migrations._6;

[Migration(version: 6)]
public class MigratePostsTableToUseIntegerId : Migration
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
