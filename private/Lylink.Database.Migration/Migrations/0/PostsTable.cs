using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Delete;

namespace Lylink.Database.Migration._0;

public static class PostsTable
{
    private const string TableName = "posts";
    private const string ParentTableName = "post_hierarchy";

    public static void DeletePosts(this IDeleteExpressionRoot delete)
    {
        delete.Table(TableName);
    }

    public static void CreatePosts(this ICreateExpressionRoot create)
    {
        create.Table(TableName)
            .WithColumn("slug")
                .AsFixedLengthString(size: 40)
                .NotNullable()
                .PrimaryKey()
            .WithColumn("title")
                .AsFixedLengthString(size: 80)
                .Nullable()
            .WithColumn("parentId")
                .AsFixedLengthString(size: 40)
                .Nullable()
                .ForeignKey("fk_parentId", ParentTableName, "categoryId")
            .WithColumn("date_modified")
                .AsDateTime()
                .WithDefault(FluentMigrator.SystemMethods.CurrentDateTime)
            .WithColumn("name")
                .AsFixedLengthString(size: 80)
                .Nullable()
            .WithColumn("keywords")
                .AsFixedLengthString(size: 160)
                .Nullable()
            .WithColumn("description")
                .AsFixedLengthString(size: 160)
                .Nullable()
            .WithColumn("body")
                .AsString(size: 60000)
                .Nullable()
            .WithColumn("date_created")
                .AsDateTime()
                .WithDefault(FluentMigrator.SystemMethods.CurrentDateTime);
    }
    /*
     CREATE TABLE posts (
  slug char(40) NOT NULL,
  title char(80) DEFAULT NULL,
  parentId char(40) DEFAULT NULL,
  date_modified datetime DEFAULT current_timestamp(),
  name char(80) DEFAULT NULL,
  keywords char(160) DEFAULT NULL,
  description char(160) DEFAULT NULL,
  body varchar(60000) DEFAULT NULL,
  date_created datetime DEFAULT current_timestamp(),
  PRIMARY KEY (slug),
  CONSTRAINT fk_parentId FOREIGN KEY (parentId) REFERENCES post_hierarchy (categoryId)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;
    */
}
