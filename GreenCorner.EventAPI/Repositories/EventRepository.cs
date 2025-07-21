using GreenCorner.EventAPI.Data;
using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Models;
using Microsoft.EntityFrameworkCore;
using GreenCorner.EventAPI.Repositories.Interface;
using Microsoft.Extensions.Logging;

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
        public async Task<IEnumerable<CleanupEvent>> GetOpenEvent()
        {
            return await _context.CleanupEvents
                                 .Where(e => e.Status == "Open")
                                 .ToListAsync();
        }
        public async Task<CleanupEvent> GetByEventId(int id)
        {
            return await _context.CleanupEvents
                .FirstOrDefaultAsync(p => p.CleanEventId == id)
                ?? throw new KeyNotFoundException($"Event with ID {id} not found.");
        }

        public async Task CreateCleanupEvent(CleanupEvent item)
        {
            await _context.CleanupEvents.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task CloseCleanupEvent(int id)
        {
            var cleanupEvent = await _context.CleanupEvents.FindAsync(id);
            if (cleanupEvent == null)
            {
                throw new KeyNotFoundException($"Event review with ID {id} not found.");
            }
            cleanupEvent.EndDate= DateTime.Now;
            cleanupEvent.Status = "Close";
            await _context.SaveChangesAsync();
        }
		public async Task OpenCleanupEvent(int id)
		{
			var cleanupEvent = await _context.CleanupEvents.FindAsync(id);
			if (cleanupEvent == null)
			{
				throw new KeyNotFoundException($"Event review with ID {id} not found.");
			}
			cleanupEvent.EndDate = null;
			cleanupEvent.Status = "Open";
			await _context.SaveChangesAsync();
		}
		public async Task UpdateCleanupEvent(CleanupEvent item)
        {
            var cleanupEvent = await _context.CleanupEvents.FindAsync(item.CleanEventId);
            if (cleanupEvent == null)
            {
                throw new KeyNotFoundException($"Cleanup Event with ID {item.CleanEventId} not found.");
            }
            _context.Entry(cleanupEvent).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCleanupEventStatus(CleanupEvent item)
        {
            var cleanupEvent = await _context.CleanupEvents.FindAsync(item.CleanEventId);
            if (cleanupEvent == null)
            {
                throw new KeyNotFoundException($"Cleanup Event with ID {item.CleanEventId} not found.");
            }
            _context.Entry(cleanupEvent).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();
        }

        public async  Task<List<CleanupEvent>> GetEventsByIdsAsync(List<int> eventIds)
        {
            return await _context.CleanupEvents
                        .Where(e => eventIds.Contains(e.CleanEventId))
                        .ToListAsync();
        }

        public async Task<int> CountVolunteersByEventIdAsync(int cleanEventId)
        {
            return await _context.EventVolunteers
                .CountAsync(ev => ev.CleanEventId == cleanEventId);
        }

        public async Task<IEnumerable<CleanupEvent>> GetTop3OpenEventsAsync()
        {
            var openEvents = await _context.CleanupEvents
                            .Where(e => e.Status == "Open")
                            .OrderBy(e => e.StartDate)
                            .Take(3)
                            .ToListAsync();

            if (!openEvents.Any())
            {
                openEvents = await _context.CleanupEvents
                    .OrderBy(e => Guid.NewGuid()) 
                    .Take(3)
                    .ToListAsync();
            }

            return openEvents;
        }
    }
}
