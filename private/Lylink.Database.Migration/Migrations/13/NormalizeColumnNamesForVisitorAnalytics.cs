using FluentMigrator;

namespace Lylink.Database.Migration._13;

[Migration(version: 13)]
public class NormalizeColumnNamesForVisitorAnalytics : FluentMigrator.Migration
{
    public override void Down()
    {
        Rename.RenameToDifferFromFailedVisitAnalytics();
    }

    public override void Up()
    {
        Rename.RenameToMatchFailedVisitAnalytics();
    }
}
