using GreenCorner.ChatAPI.Data;
using GreenCorner.ChatAPI.Models;
using GreenCorner.ChatAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.ChatAPI.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ChatDbContext _context;

        public NotificationRepository(ChatDbContext context)
        {
            _context = context;
        }

        public async Task SaveNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetNotificationsByUserIdAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }
    }
}

