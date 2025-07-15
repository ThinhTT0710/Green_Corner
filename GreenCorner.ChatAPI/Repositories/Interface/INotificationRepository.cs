using GreenCorner.ChatAPI.Models;

namespace GreenCorner.ChatAPI.Repositories.Interface
{
    public interface INotificationRepository
    {
        Task SaveNotificationAsync(Notification notification);
        Task<List<Notification>> GetNotificationsByUserIdAsync(string userId);
    }
}
