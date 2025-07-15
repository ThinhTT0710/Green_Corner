using GreenCorner.ChatAPI.DTOs;

namespace GreenCorner.ChatAPI.Services.Interface
{
    public interface IChatService
    {
        Task SaveMessageAsync(ChatMessageDTO message);
        Task<List<ChatMessageDTO>> GetMessagesByEventIdAsync(int eventId);
    }
}
