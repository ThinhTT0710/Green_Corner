using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace GreenCorner.ChatAPI.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                Console.WriteLine($"[NotificationHub] Connected: {Context.ConnectionId}, UserId: {userId}");
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }

            await base.OnConnectedAsync();
        }
    }
}
