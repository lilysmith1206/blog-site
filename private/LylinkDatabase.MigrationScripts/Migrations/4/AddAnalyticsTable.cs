using FluentMigrator;

namespace LylinkDatabase.MigrationScripts.Migrations._4;

[Migration(version: 4)]
public class AddAnalyticsTable : Migration
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
