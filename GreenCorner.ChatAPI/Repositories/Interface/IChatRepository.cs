using GreenCorner.ChatAPI.Models;

namespace GreenCorner.ChatAPI.Repositories.Interface
{
    public interface IChatRepository
    {
        Task SaveMessageAsync(ChatMessage message);
        Task<List<ChatMessage>> GetMessagesByEventIdAsync(int eventId);
    }
}
