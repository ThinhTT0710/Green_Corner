using AutoMapper;
using GreenCorner.ChatAPI.DTOs;
using GreenCorner.ChatAPI.Hubs;
using GreenCorner.ChatAPI.Models;
using GreenCorner.ChatAPI.Repositories.Interface;
using GreenCorner.ChatAPI.Services.Interface;
using Microsoft.AspNetCore.SignalR;

namespace GreenCorner.ChatAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository repo, IHubContext<NotificationHub> hubContext, IMapper mapper)
        {
            _repo = repo;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        public async Task SendNotificationAsync(string userId, string title, string message)
        {
            var noti = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message
            };

            await _repo.SaveNotificationAsync(noti);

            await _hubContext.Clients.Group(userId).SendAsync("ReceiveNotification", new
            {
                title,
                message,
                createdAt = noti.CreatedAt.ToString("HH:mm dd/MM/yyyy")
            });
        }

        public async Task<List<NotificationDTO>> GetNotificationsByUserIdAsync(string userId)
        {
            var list = await _repo.GetNotificationsByUserIdAsync(userId);
            return _mapper.Map<List<NotificationDTO>>(list);
        }
    }
}
