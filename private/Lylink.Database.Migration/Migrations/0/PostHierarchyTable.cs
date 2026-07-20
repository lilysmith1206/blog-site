using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Delete;

namespace Lylink.Database.Migration._0;

public static class PostHierarchyTable
{
    private const string TableName = "post_hierarchy";

    public static void DeletePostHierarchies(this IDeleteExpressionRoot delete)
    {
        delete.Table(TableName);
    }

    public static void CreatePostHierarchies(this ICreateExpressionRoot create)
    {
        create.Table(TableName)
            .WithColumn("categoryId")
                .AsString(size: 40)
                .NotNullable()
                .PrimaryKey()
            .WithColumn("parentId")
                .AsString(size: 40)
                .Nullable()
            .WithColumn("name")
                .AsString(size: 40)
                .Nullable()
            .WithColumn("slug")
                .AsDateTime()
                .WithDefault(FluentMigrator.SystemMethods.CurrentDateTime)
            .WithColumn("title")
                .AsString(size: 80)
                .Nullable()
            .WithColumn("keywords")
                .AsString(size: 160)
                .Nullable()
            .WithColumn("description")
                .AsString(size: 160)
                .Nullable()
            .WithColumn("body")
                .AsString(size: 60000)
                .Nullable()
            .WithColumn("use_date_created_for_sorting")
                .AsDateTime()
                .WithDefault(FluentMigrator.SystemMethods.CurrentDateTime);
    }

    /*
    CREATE TABLE post_hierarchy(
  categoryId char(40) NOT NULL,
  parentId char (40) DEFAULT NULL,
  name char (80) DEFAULT NULL,
  slug char (40) DEFAULT NULL,
  title char (80) DEFAULT NULL,
  keywords char (80) DEFAULT NULL,
  description char (80) DEFAULT NULL,
  body varchar(60000) DEFAULT NULL,
  use_date_created_for_sorting tinyint(1) DEFAULT NULL,
  PRIMARY KEY(categoryId)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE = latin1_swedish_ci;
    */
}
