using GreenCorner.ChatAPI.DTOs;

namespace GreenCorner.ChatAPI.Services.Interface
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string userId, string title, string message);
        Task<List<NotificationDTO>> GetNotificationsByUserIdAsync(string userId);
    }
}