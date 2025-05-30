using System;
using System.Collections.Generic;
using GreenCorner.BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.BlogAPI.Data;

public partial class GreenCornerBlogContext : DbContext
{
    public GreenCornerBlogContext()
    {
    }

    public GreenCornerBlogContext(DbContextOptions<GreenCornerBlogContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BlogFavorite> BlogFavorites { get; set; }

    public virtual DbSet<BlogPost> BlogPosts { get; set; }

    public virtual DbSet<BlogReport> BlogReports { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=MSI;Initial Catalog=GreenCorner_Blog;User ID=sa;Password=123;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogFavorite>(entity =>
        {
            entity.HasKey(e => e.BlogFavoriteId).HasName("PK__BlogFavo__B6FD66088468CD98");

            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogFavorites)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BlogFavorites_BlogPosts");
        });

        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK__BlogPost__54379E30B44D6CF2");

            entity.Property(e => e.AuthorId).HasMaxLength(450);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.ThumbnailUrl).HasMaxLength(1000);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<BlogReport>(entity =>
        {
            entity.HasKey(e => e.BlogReportId).HasName("PK__BlogRepo__EC44CA9207A93957");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogReports)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BlogReports_BlogPosts");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedBackId).HasName("PK__Feedback__E2CB3B87A1A4109F");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Report__D5BD4805BA0FB58D");

            entity.ToTable("Report");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.LeaderId).HasMaxLength(450);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
