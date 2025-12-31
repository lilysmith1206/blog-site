using FluentMigrator;

namespace LylinkDatabase.MigrationScripts.Migrations._11;

[Migration(version: 11)]
public class DropFailedVisitsFromVisitAnalytics : Migration
{
    public override void Down()
    {
        Execute.CopyFailedVisitsFromFailedVisitsTable();
    }

    public override void Up()
    {
        Delete.DropFailedVisitsFromVisitAnalytics();
    }
}
