using GreenCorner.ChatAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GreenCorner.ChatAPI.Data
{
    public class ChatDbContext : DbContext
    {
            public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
            {
            }
            public DbSet<ChatMessage> ChatMessages { get; set; }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);
                builder.Entity<ChatMessage>(entity =>
                {
                    entity.HasKey(e => e.ChatMessageId);

                    entity.Property(e => e.SenderId)
                          .IsRequired();

                    entity.Property(e => e.SenderName)
                          .IsRequired()
                          .HasMaxLength(100);

                    entity.Property(e => e.SenderAvatar)
                          .HasMaxLength(250);

                    entity.Property(e => e.Content)
                          .IsRequired();

                    entity.Property(e => e.Timestamp)
                          .IsRequired();
                });
            }
    }
}
