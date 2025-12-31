using FluentMigrator;

namespace LylinkDatabase.MigrationScripts.Migrations._12;

[Migration(version: 12)]
public class DropGivenAndVisitedStructureForSuccessVisit : Migration
{
    public override void Down()
    {
        Create.CreateSlugGivenColumn();
        Execute.SetSlugGivenToSlugVisited();
    }

    public override void Up()
    {
        Delete.DropSlugGivenColumn();
    }
}
