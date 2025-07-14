using GreenCorner.ChatAPI.DTOs;
using GreenCorner.ChatAPI.Models;
using GreenCorner.ChatAPI.Services;
using GreenCorner.ChatAPI.Services.Interface;
using Microsoft.AspNetCore.SignalR;

namespace GreenCorner.ChatAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task SendMessage(string eventId, string senderId, string senderName, string senderAvatar, string content)
        {
            var message = new ChatMessageDTO
            {
                EventId = int.Parse(eventId),
                SenderId = senderId,
                SenderName = senderName,
                SenderAvatar = senderAvatar,
                Content = content,
                Timestamp = DateTime.UtcNow
            };

            await _chatService.SaveMessageAsync(message);

            await Clients.Group(eventId).SendAsync("ReceiveMessage", new
            {
                senderId,
                senderName,
                senderAvatar,
                content,
                timestamp = message.Timestamp.ToString("HH:mm:ss dd/MM/yyyy")
            });
        }

        public override async Task OnConnectedAsync()
        {
            var eventId = Context.GetHttpContext()?.Request.Query["eventId"];
            if (!string.IsNullOrEmpty(eventId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, eventId);
            }
            await base.OnConnectedAsync();
        }
    }
}
