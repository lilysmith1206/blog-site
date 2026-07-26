using FluentMigrator;

namespace Lylink.Database.Migration._10;

[Migration(version: 10)]
public class MigrateInvalidPageVisitsToSeparateTable : FluentMigrator.Migration
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
