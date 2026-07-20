using FluentMigrator;

namespace Lylink.Database.Migration._12;

[Migration(version: 12)]
public class DropGivenAndVisitedStructureForSuccessVisit : FluentMigrator.Migration
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
