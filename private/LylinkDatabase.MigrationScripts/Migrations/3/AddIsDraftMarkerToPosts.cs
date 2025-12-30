using FluentMigrator;

namespace LylinkDatabase.MigrationScripts.Migrations._3;


[Migration(version: 3)]
public class AddIsDraftMarkerToPosts : Migration
{
    public override void Down()
    {
        Delete.RemoveIsDraftColumn();
    }

    public override void Up()
    {
        Alter.AddIsDraftColumn();
    }
}
