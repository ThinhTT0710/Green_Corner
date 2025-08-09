using GreenCorner.EventAPI.Data;
using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.EventAPI.Repositories
{
    public class EventReviewRepository : IEventReviewRepository
    {
        private readonly GreenCornerEventContext _context;
        public EventReviewRepository(GreenCornerEventContext context)
        {
            _context = context;
        }
        public async Task RateEvent(EventReview item)
        {
            await _context.EventReviews.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventReview(int id)
        {
            var eventReview = await _context.EventReviews.FindAsync(id);
            if (eventReview == null)
            {
                throw new KeyNotFoundException($"Event review with ID {id} not found.");
            }
            //product.IsDeleted = true;
            _context.EventReviews.Remove(eventReview);
            await _context.SaveChangesAsync();
        }
        public async Task EditEventReview(EventReview item)
        {
            var eventReview = await _context.EventReviews.FindAsync(item.EventReviewId);
            if (eventReview == null)
            {
                throw new KeyNotFoundException($"Event review with ID {item.EventReviewId} not found.");
            }
            _context.Entry(eventReview).CurrentValues.SetValues(item);
            eventReview.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<EventReview>> ViewEventReviewHistory(string userID)
        {
            return await _context.EventReviews
                         .Where(e => e.UserId == userID)
                         .ToListAsync();
        }
        public async Task<EventReview> GetEventReviewById(int id)
        {
            return await _context.EventReviews
                .FirstOrDefaultAsync(p => p.EventReviewId == id)
                ?? throw new KeyNotFoundException($"Event review with ID {id} not found.");
        }
        public async Task<IEnumerable<EventReview>> GetEventReviewsByEventIdAsync(int eventId)
        {
            return await _context.EventReviews
                .Where(r => r.CleanEventId == eventId)
                .ToListAsync();
        }
    }
}
