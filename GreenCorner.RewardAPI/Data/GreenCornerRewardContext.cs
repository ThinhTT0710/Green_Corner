using System;
using System.Collections.Generic;
using GreenCorner.RewardAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.RewardAPI.Data;

public partial class GreenCornerRewardContext : DbContext
{
    public GreenCornerRewardContext()
    {
    }

    public GreenCornerRewardContext(DbContextOptions<GreenCornerRewardContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PointTransaction> PointTransactions { get; set; }

    public virtual DbSet<RewardPoint> RewardPoints { get; set; }

    public virtual DbSet<UserVoucherRedemption> UserVoucherRedemptions { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PointTransaction>(entity =>
        {
            entity.HasKey(e => e.PointTransactionsId).HasName("PK__PointTra__BBA48425F1272BE3");

            entity.Property(e => e.CleanEventId).HasDefaultValue(0);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        modelBuilder.Entity<RewardPoint>(entity =>
        {
            entity.HasKey(e => e.RewardPointsId).HasName("PK__RewardPo__8CC9006655C351A0");

            entity.Property(e => e.UserId).HasMaxLength(450);
        });

        modelBuilder.Entity<UserVoucherRedemption>(entity =>
        {
            entity.HasKey(e => e.UserVoucherId).HasName("PK__UserVouc__8017D49926C7E418");

            entity.Property(e => e.RedeemedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.Voucher).WithMany(p => p.UserVoucherRedemptions)
                .HasForeignKey(d => d.VoucherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserVoucherRedemptions_Voucher");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.VoucherId).HasName("PK__Vouchers__3AEE7921C707E647");

            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
