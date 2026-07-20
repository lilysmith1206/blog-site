using FluentMigrator;

namespace Lylink.Database.Migration._3;


[Migration(version: 3)]
public class AddIsDraftMarkerToPosts : FluentMigrator.Migration
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
