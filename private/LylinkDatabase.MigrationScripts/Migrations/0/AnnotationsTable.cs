using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Delete;

namespace LylinkDatabase.MigrationScripts.Migrations._0;

public static class AnnotationsTable
{
    private const string TableName = "annotations";

    public static void DeleteAnnotationsTable(this IDeleteExpressionRoot delete)
    {
        delete.Table(TableName);
    }

    public static void CreateAnnotationsTable(this ICreateExpressionRoot create)
    {
        create.Table(TableName)
            .WithColumn("id")
                .AsString(size: 40)
                .NotNullable()
                .PrimaryKey()
            .WithColumn("slug")
                .AsString(size: 40)
                .Nullable()
            .WithColumn("editor_name")
                .AsString(size: 40)
                .Nullable()
            .WithColumn("annotation_content")
                .AsString(size: 10000)
                .Nullable();
    }
}
/*
CREATE TABLE annotations (
  id varchar(40) NOT NULL,
  slug varchar(40) DEFAULT NULL,
  editor_name varchar(80) DEFAULT NULL,
  annotation_content varchar(10000) DEFAULT NULL,
  PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;
*/
