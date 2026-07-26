using FluentMigrator;

namespace Lylink.Database.Migration._9;

[Migration(version: 9)]
public class DropAnnotationsTable : FluentMigrator.Migration
{
    public override void Down()
    {
        Create.CreateAnnotationsTable();
    }

    public override void Up()
    {
        Delete.DropAnnotationsTable();
    }
}
