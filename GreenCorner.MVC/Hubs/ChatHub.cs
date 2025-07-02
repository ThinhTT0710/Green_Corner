using GreenCorner.MVC.Services.Interface;
using Microsoft.AspNetCore.SignalR;

namespace GreenCorner.MVC.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", user, message);

            var reply = await _chatService.GetGeminiResponseAsync(message);
            await Clients.Caller.SendAsync("ReceiveMessage", "Trợ lý ảo", reply);
        }
    }
}
