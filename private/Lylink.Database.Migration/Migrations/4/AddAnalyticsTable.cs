using FluentMigrator;

namespace Lylink.Database.Migration._4;

[Migration(version: 4)]
public class AddAnalyticsTable : FluentMigrator.Migration
{
    public override void Down()
    {
        Delete.RemoveAnalyticsTable();
    }

    public override void Up()
    {
        Create.AddAnalyticsTable();
    }
}
