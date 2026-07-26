using FluentMigrator;

namespace Lylink.Database.Migration._2;

[Migration(version: 2)]
public class RenamePostHierarchyToPostCategory : FluentMigrator.Migration
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
