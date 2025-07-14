using GreenCorner.MVC.Models;
using GreenCorner.MVC.Models.Chat;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IChatService
    {
        Task<string> GetGeminiResponseAsync(string message);
        Task<ResponseDTO?> GetChatMessagesAsync(int eventId);
    }
}
