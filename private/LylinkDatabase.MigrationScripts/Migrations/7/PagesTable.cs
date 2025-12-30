using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Delete;

namespace LylinkDatabase.MigrationScripts.Migrations._7;

public static class PagesTable
{
    public static void CreatePagesTable(this ICreateExpressionRoot create)
    {
        create.Table(MigrationConstants.PagesTableName)
            .WithColumn("slug")
                .AsFixedLengthString(size: 40)
                .NotNullable()
                .PrimaryKey()
            .WithColumn("title")
                .AsFixedLengthString(size: 80)
                .NotNullable()
            .WithColumn("name")
                .AsFixedLengthString(size: 80)
                .NotNullable()
            .WithColumn("keywords")
                .AsFixedLengthString(size: 160)
                .NotNullable()
            .WithColumn("description")
                .AsFixedLengthString(size: 160)
                .NotNullable()
            .WithColumn("body")
                .AsString(size: 60000)
                .NotNullable();
        /*
          slug CHAR(40) NOT NULL,
          title CHAR(80) NOT NULL,
          name CHAR(80) NOT NULL,
          keywords CHAR(160) NOT NULL,
          description CHAR(160) NOT NULL,
          body TEXT NOT NULL,
        */
    }

    public static void DeletePagesTable(this IDeleteExpressionRoot delete)
    {
        delete.Table(MigrationConstants.PagesTableName);
    }
}
