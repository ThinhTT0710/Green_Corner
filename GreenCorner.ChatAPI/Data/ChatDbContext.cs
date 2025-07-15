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
            public DbSet<Notification> Notifications { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);
            }
    }
}
