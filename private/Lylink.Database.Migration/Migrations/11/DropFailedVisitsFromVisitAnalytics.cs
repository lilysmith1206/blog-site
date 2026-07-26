using FluentMigrator;

namespace Lylink.Database.Migration._11;

[Migration(version: 11)]
public class DropFailedVisitsFromVisitAnalytics : FluentMigrator.Migration
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
