using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;

namespace LylinkBackend_DatabaseAccessLayer.Models;

public partial class LylinkdbContext : DbContext
{
    private readonly string? _connectionString;

    public LylinkdbContext()
    {
    }

    public LylinkdbContext(DbContextOptions<LylinkdbContext> options)
        : base(options)
    {
        _connectionString = options.Extensions
            .OfType<MySqlOptionsExtension>()
            .FirstOrDefault()?.ConnectionString;
    }

    public virtual DbSet<Annotation> Annotations { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostCategory> PostCategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured == false)
        {
            if (string.IsNullOrEmpty(_connectionString) == false)
            {
                optionsBuilder.UseMySql(_connectionString, ServerVersion.Parse("11.5.2-mariadb"));
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<Annotation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("annotations")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

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

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Slug).HasName("PRIMARY");

            entity.ToTable("posts");

            entity.HasIndex(e => e.ParentId, "fk_posts_parent");

            entity.Property(e => e.Slug)
                .HasMaxLength(40)
                .IsFixedLength()
                .HasColumnName("slug");
            entity.Property(e => e.Body)
                .HasColumnType("varchar(60000)")
                .HasColumnName("body");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("date_modified");
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
            entity.Property(e => e.ParentId)
                .HasColumnType("int(11)")
                .HasColumnName("parentId");
            entity.Property(e => e.Title)
                .HasMaxLength(80)
                .IsFixedLength()
                .HasColumnName("title");

            entity.HasOne(d => d.Parent).WithMany(p => p.Posts)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_posts_parent");
        });

        modelBuilder.Entity<PostCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("post_categories");

            entity.HasIndex(e => e.ParentId, "fk_post_hierarchy_parent");

            entity.Property(e => e.CategoryId)
                .HasColumnType("int(11)")
                .HasColumnName("categoryId");
            entity.Property(e => e.Body)
                .HasColumnType("varchar(60000)")
                .HasColumnName("body");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(80)
                .IsFixedLength()
                .HasColumnName("categoryName");
            entity.Property(e => e.Description)
                .HasMaxLength(80)
                .IsFixedLength()
                .HasColumnName("description");
            entity.Property(e => e.Keywords)
                .HasMaxLength(80)
                .IsFixedLength()
                .HasColumnName("keywords");
            entity.Property(e => e.ParentId)
                .HasColumnType("int(11)")
                .HasColumnName("parentId");
            entity.Property(e => e.Slug)
                .HasMaxLength(40)
                .IsFixedLength()
                .HasColumnName("slug");
            entity.Property(e => e.Title)
                .HasMaxLength(80)
                .IsFixedLength()
                .HasColumnName("title");
            entity.Property(e => e.UseDateCreatedForSorting).HasColumnName("use_date_created_for_sorting");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_post_hierarchy_parent");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
