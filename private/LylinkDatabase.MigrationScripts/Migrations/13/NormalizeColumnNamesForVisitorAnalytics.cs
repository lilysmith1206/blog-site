using FluentMigrator;

namespace LylinkDatabase.MigrationScripts.Migrations._13;

[Migration(version: 13)]
public class NormalizeColumnNamesForVisitorAnalytics : Migration
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
