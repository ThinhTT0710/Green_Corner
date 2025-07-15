using GreenCorner.MVC.Models;
using GreenCorner.MVC.Models.Notification;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;

namespace GreenCorner.MVC.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IBaseService _baseService;
        public NotificationService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO?> SendNotification(NotificationDTO notificationDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = notificationDTO,
                Url = SD.ChatAPIBase + "/api/notifications/send"
            });
        }

    }
}
