using FluentMigrator;

namespace LylinkDatabase.MigrationScripts.Migrations._9;

[Migration(version: 9)]
public class DropAnnotationsTable : Migration
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
