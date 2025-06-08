using GreenCorner.EventAPI.Data;
using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Models;
using Microsoft.EntityFrameworkCore;
using GreenCorner.EventAPI.Repositories.Interface;

namespace GreenCorner.EventAPI.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly GreenCornerEventContext _context;
        public EventRepository(GreenCornerEventContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CleanupEvent>> GetAllEvent()
        {
            return await _context.CleanupEvents.ToListAsync();
        }

        public async Task<CleanupEvent> GetByEventId(int id)
        {
            return await _context.CleanupEvents
                .FirstOrDefaultAsync(p => p.CleanEventId == id)
                ?? throw new KeyNotFoundException($"Event with ID {id} not found.");
        }

        
    }
}
