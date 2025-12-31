using FluentMigrator;

namespace LylinkDatabase.MigrationScripts.Migrations._10;

[Migration(version: 10)]
public class MigrateInvalidPageVisitsToSeparateTable : Migration
{
    public override void Down()
    {
        Delete.DropFailedVisitTable();
    }

    public override void Up()
    {
        Create.CreateFailedVisitTable();
        Execute.Copy404GivenVisitsToFailedTable();
    }
}
