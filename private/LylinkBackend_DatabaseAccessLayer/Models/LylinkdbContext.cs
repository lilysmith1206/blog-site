using Microsoft.EntityFrameworkCore;

namespace LylinkBackend_DatabaseAccessLayer.Models;

public partial class LylinkdbContext : DbContext
{
    public virtual DbSet<Annotation> Annotations { get; set; }

    public virtual DbSet<DatabaseVersion> DatabaseVersions { get; set; }

    public virtual DbSet<Page> Pages { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostCategory> PostCategories { get; set; }

    public virtual DbSet<VisitAnalytic> VisitAnalytics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<Annotation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("annotations");

            entity.Property(e => e.Id)
                .HasMaxLength(40)
                .HasColumnName("id");
            entity.Property(e => e.AnnotationContent)
                .HasMaxLength(10000)
                .HasColumnName("annotation_content");
            entity.Property(e => e.EditorName)
                .HasMaxLength(80)
                .HasColumnName("editor_name");
            entity.Property(e => e.Slug)
                .HasMaxLength(40)
                .HasColumnName("slug");
        });

        modelBuilder.Entity<DatabaseVersion>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("database_version")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.UpdatedOn)
                .HasColumnType("datetime")
                .HasColumnName("updated_on");
            entity.Property(e => e.Version)
                .HasMaxLength(3)
                .IsFixedLength()
                .HasColumnName("version");
        });

        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasKey(e => e.Slug).HasName("PRIMARY");

            entity.ToTable("pages");

            entity.Property(e => e.Slug)
                .HasMaxLength(40)
                .IsFixedLength()
                .HasColumnName("slug");
            entity.Property(e => e.Body)
                .HasColumnType("text")
                .HasColumnName("body");
            entity.Property(e => e.Description)
                .HasMaxLength(160)
                .IsFixedLength()
                .HasColumnName("description");
            entity.Property(e => e.Keywords)
                .HasMaxLength(160)
                .IsFixedLength()
                .HasColumnName("keywords");
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsFixedLength()
                .HasColumnName("name");
            entity.Property(e => e.Title)
                .HasMaxLength(80)
                .IsFixedLength()
                .HasColumnName("title");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("posts");

            entity.HasIndex(e => e.ParentId, "key_post_parent_category");

            entity.HasIndex(e => e.Slug, "slug").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
            entity.Property(e => e.IsDraft).HasColumnName("is_draft");
            entity.Property(e => e.ParentId)
                .HasColumnType("int(11)")
                .HasColumnName("parent_id");
            entity.Property(e => e.Slug)
                .HasMaxLength(40)
                .IsFixedLength()
                .HasColumnName("slug");

            entity.HasOne(d => d.Parent).WithMany(p => p.Posts)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("foreign_key_post_parent_category");

            entity.HasOne(d => d.SlugNavigation).WithOne(p => p.Post)
                .HasForeignKey<Post>(d => d.Slug)
                .HasConstraintName("fk_posts_pages");
        });

        modelBuilder.Entity<PostCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("post_categories");

            entity.HasIndex(e => e.Slug, "fk_post_categories_pages");

            entity.HasIndex(e => e.ParentId, "fk_post_hierarchy_parent");

            entity.Property(e => e.CategoryId)
                .HasColumnType("int(11)")
                .HasColumnName("categoryId");
            entity.Property(e => e.ParentId)
                .HasColumnType("int(11)")
                .HasColumnName("parentId");
            entity.Property(e => e.Slug)
                .HasMaxLength(40)
                .IsFixedLength()
                .HasColumnName("slug");
            entity.Property(e => e.UseDateCreatedForSorting).HasColumnName("use_date_created_for_sorting");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_post_hierarchy_parent");

            entity.HasOne(d => d.SlugNavigation).WithMany(p => p.PostCategories)
                .HasForeignKey(d => d.Slug)
                .HasConstraintName("fk_post_categories_pages");
        });

        modelBuilder.Entity<VisitAnalytic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("visit_analytics")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.SlugGiven)
                .HasMaxLength(40)
                .IsFixedLength()
                .HasColumnName("slug_given");
            entity.Property(e => e.SlugVisited)
                .HasColumnType("text")
                .HasColumnName("slug_visited");
            entity.Property(e => e.VisitedOn)
                .HasColumnType("datetime")
                .HasColumnName("visited_on");
            entity.Property(e => e.VisitorId)
                .HasMaxLength(128)
                .IsFixedLength()
                .HasColumnName("visitor_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
