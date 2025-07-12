using GreenCorner.EventAPI.Data;
using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.EventAPI.Repositories
{
    public class TrashEventRepository : ITrashEventRepository
    {
        private readonly GreenCornerEventContext _context;

        public TrashEventRepository(GreenCornerEventContext context)
        {
            _context = context;
        }

        public async Task AddTrashEvent(TrashEvent trashEvent)
        {
            await _context.TrashEvents.AddAsync(trashEvent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTrashEvent(int id)
        {
            var trashEvent = await _context.TrashEvents.FindAsync(id);
            if (trashEvent == null)
            {
                throw new KeyNotFoundException($"TrashEvent with ID {id} not found.");
            }
            _context.TrashEvents.Remove(trashEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TrashEvent>> GetAllTrashEvent()
        {
            return await _context.TrashEvents.ToListAsync();
        }

        public async Task<TrashEvent> GetByTrashEventId(int id)
        {
            return await _context.TrashEvents
                .FirstOrDefaultAsync(p => p.TrashReportId == id)
                ?? throw new KeyNotFoundException($"TrashEvent with ID {id} not found.");
        }

        public async Task UpdateTrashEvent(TrashEvent trashEvent)
        {
            var trashevent = await _context.TrashEvents.FindAsync(trashEvent.TrashReportId);
            if (trashevent == null)
            {
                throw new KeyNotFoundException($"TrashEvent with ID {trashEvent.TrashReportId} not found.");
            }
            _context.Entry(trashevent).CurrentValues.SetValues(trashEvent);
            await _context.SaveChangesAsync();
        }

        public async Task ApproveTrashEvent(int id)
        {
            var trashEvent = await _context.TrashEvents.FindAsync(id);
            if (trashEvent == null)
            {
                throw new Exception($"TrashEvent with ID {id} not found.");
            }
            if (trashEvent.Status == "Đã từ chối")
            {
                throw new Exception($"Sự kiện này đã bị từ chối trước đó");
            }
            trashEvent.Status = "Đã xác nhận";
            await _context.SaveChangesAsync();
        }

        public async Task RejectTrashEvent(int id)
        {
            var trashEvent = await _context.TrashEvents.FindAsync(id);
            if (trashEvent == null)
            {
                throw new KeyNotFoundException($"TrashEvent with ID {id} not found.");
            }
            if (trashEvent.Status == "Đã xác nhận")
            {
                throw new Exception($"Sự kiện này đã được xác nhận trước đó");
            }
            trashEvent.Status = "Đã từ chối";
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TrashEvent>> GetByUserId(string userId)
        {
            return await _context.TrashEvents
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }
    }
}
