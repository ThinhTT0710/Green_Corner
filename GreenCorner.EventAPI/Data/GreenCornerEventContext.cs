using System;
using System.Collections.Generic;
using GreenCorner.EventAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.EventAPI.Data;

public partial class GreenCornerEventContext : DbContext
{
    public GreenCornerEventContext()
    {
    }

    public GreenCornerEventContext(DbContextOptions<GreenCornerEventContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CleanupEvent> CleanupEvents { get; set; }

    public virtual DbSet<EventReview> EventReviews { get; set; }

    public virtual DbSet<EventVolunteer> EventVolunteers { get; set; }

    public virtual DbSet<LeaderReview> LeaderReviews { get; set; }

    public virtual DbSet<TrashEvent> TrashEvents { get; set; }

    public virtual DbSet<Volunteer> Volunteers { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CleanupEvent>(entity =>
        {
            entity.HasKey(e => e.CleanEventId).HasName("PK__CleanupE__D8604A1F50262743");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<EventReview>(entity =>
        {
            entity.HasKey(e => e.EventReviewId).HasName("PK__EventRev__F0772CCE58C3BE7B");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.CleanEvent).WithMany(p => p.EventReviews)
                .HasForeignKey(d => d.CleanEventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventReviews_CleanupEvents");
        });

        modelBuilder.Entity<EventVolunteer>(entity =>
        {
            entity.HasKey(e => e.EventVolunteerId).HasName("PK__EventVol__66FAD756FA2392FC");

            entity.Property(e => e.AttendanceStatus).HasMaxLength(50);
            entity.Property(e => e.JoinDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.CleanEvent).WithMany(p => p.EventVolunteers)
                .HasForeignKey(d => d.CleanEventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventVolunteers_CleanupEvents");
        });

        modelBuilder.Entity<LeaderReview>(entity =>
        {
            entity.HasKey(e => e.LeaderReviewId).HasName("PK__LeaderRe__68C42BA3CE506E1A");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.LeaderId).HasMaxLength(450);
            entity.Property(e => e.ReviewerId).HasMaxLength(450);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TrashEvent>(entity =>
        {
            entity.HasKey(e => e.TrashReportId).HasName("PK__TrashEve__43E6EF261EA3457E");

            entity.ToTable("TrashEvent");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasMaxLength(1000);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        modelBuilder.Entity<Volunteer>(entity =>
        {
            entity.HasKey(e => e.VolunteerId).HasName("PK__Voluntee__716F6F2CFE61F34A");

            entity.Property(e => e.ApplicationType).HasMaxLength(50);
            entity.Property(e => e.Assignment).HasMaxLength(100);
            entity.Property(e => e.CarryItems).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UserId).HasMaxLength(450);
            entity.HasOne(d => d.CleanEvent).WithMany(p => p.Volunteers)
                .HasForeignKey(d => d.CleanEventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Volunteers_CleanupEvents");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
