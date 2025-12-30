using FluentMigrator;

namespace LylinkDatabase.MigrationScripts.Migrations._2;

[Migration(version: 2)]
public class RenamePostHierarchyToPostCategory : Migration
{
    public override void Down()
    {
        Execute.RenameToPostHierarchy();
    }

    public override void Up()
    {
        Execute.RenameToPostCategory();
    }
}
