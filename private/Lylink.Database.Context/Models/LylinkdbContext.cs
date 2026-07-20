using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Lylink.Database.Context.Models;

public partial class LylinkdbContext : DbContext
{
    public LylinkdbContext()
    {
    }

    public LylinkdbContext(DbContextOptions<LylinkdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FailedVisitAnalytic> FailedVisitAnalytics { get; set; }

    public virtual DbSet<Page> Pages { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostCategory> PostCategories { get; set; }

    public virtual DbSet<PostSortingMethod> PostSortingMethods { get; set; }

    public virtual DbSet<Versioninfo> Versioninfos { get; set; }

    public virtual DbSet<VisitAnalytic> VisitAnalytics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<FailedVisitAnalytic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("failed_visit_analytics");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.AttemptedSlug)
                .HasMaxLength(255)
                .HasColumnName("attempted_slug");
            entity.Property(e => e.DateCreated)
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.RedirectedSlug)
                .HasMaxLength(255)
                .HasColumnName("redirected_slug");
            entity.Property(e => e.SessionId)
                .HasMaxLength(128)
                .IsFixedLength()
                .HasColumnName("session_id");
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

            entity.HasIndex(e => e.PostSortingMethodId, "fk_category_post_sorting_method");

            entity.HasIndex(e => e.Slug, "fk_post_categories_pages");

            entity.HasIndex(e => e.ParentId, "fk_post_hierarchy_parent");

            entity.Property(e => e.CategoryId)
                .HasColumnType("int(11)")
                .HasColumnName("categoryId");
            entity.Property(e => e.ParentId)
                .HasColumnType("int(11)")
                .HasColumnName("parentId");
            entity.Property(e => e.PostSortingMethodId)
                .HasColumnType("int(11)")
                .HasColumnName("post_sorting_method_id");
            entity.Property(e => e.Slug)
                .HasMaxLength(40)
                .IsFixedLength()
                .HasColumnName("slug");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_post_hierarchy_parent");

            entity.HasOne(d => d.PostSortingMethod).WithMany(p => p.PostCategories)
                .HasForeignKey(d => d.PostSortingMethodId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_category_post_sorting_method");

            entity.HasOne(d => d.SlugNavigation).WithMany(p => p.PostCategories)
                .HasForeignKey(d => d.Slug)
                .HasConstraintName("fk_post_categories_pages");
        });

        modelBuilder.Entity<PostSortingMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("post_sorting_methods");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.SortingName)
                .HasMaxLength(80)
                .IsFixedLength()
                .HasColumnName("sorting_name");
        });

        modelBuilder.Entity<Versioninfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("versioninfo");

            entity.HasIndex(e => e.Version, "UC_Version").IsUnique();

            entity.Property(e => e.AppliedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1024);
            entity.Property(e => e.Version).HasColumnType("bigint(20)");
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
            entity.Property(e => e.DateCreated)
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.SessionId)
                .HasMaxLength(128)
                .IsFixedLength()
                .HasColumnName("session_id");
            entity.Property(e => e.VisitedSlug)
                .HasColumnType("text")
                .HasColumnName("visited_slug");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
