using GreenCorner.MVC.Models;
using GreenCorner.MVC.Models.Notification;

namespace GreenCorner.MVC.Services.Interface
{
    public interface INotificationService
    {
        Task<ResponseDTO?> SendNotification(NotificationDTO notificationDTO);
    }
}
