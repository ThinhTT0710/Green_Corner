using GreenCorner.ChatAPI.Data;
using GreenCorner.ChatAPI.Models;
using GreenCorner.ChatAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.ChatAPI.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ChatDbContext _context;

        public ChatRepository(ChatDbContext context)
        {
            _context = context;
        }

        public async Task SaveMessageAsync(ChatMessage message)
        {
            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ChatMessage>> GetMessagesByEventIdAsync(int eventId)
        {
            return await _context.ChatMessages
                .Where(m => m.EventId == eventId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }
    }
}
