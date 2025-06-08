using GreenCorner.EventAPI.Data;
using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.EventAPI.Repositories
{
    public class LeaderReviewRepository : ILeaderReviewRepository
    {
        private readonly GreenCornerEventContext _context;
        public LeaderReviewRepository(GreenCornerEventContext context)
        {
            _context = context;
        }
        public async Task LeaderReview(LeaderReview item)
        {
            item.CreatedAt = DateTime.Now;
            await _context.LeaderReviews.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLeaderReview(int id)
        {
            var leaderReview = await _context.LeaderReviews.FindAsync(id);
            if (leaderReview == null)
            {
                throw new KeyNotFoundException($"Leader review with ID {id} not found.");
            }
            //product.IsDeleted = true;
            _context.LeaderReviews.Remove(leaderReview);
            await _context.SaveChangesAsync();
        }
        public async Task EditLeaderReview(LeaderReview item)
        {
            var leaderReview = await _context.LeaderReviews.FindAsync(item.LeaderReviewId);
            if (leaderReview == null)
            {
                throw new KeyNotFoundException($"Leader review with ID {item.LeaderReviewId} not found.");
            }
            _context.Entry(leaderReview).CurrentValues.SetValues(item);
            leaderReview.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<LeaderReview>> ViewLeaderReviewHistory(string reviewerID)
        {
            return await _context.LeaderReviews
                         .Where(e => e.ReviewerId == reviewerID)
                         .ToListAsync();
        }
		public async Task<LeaderReview> GetLeaderReviewById(int id)
		{
			return await _context.LeaderReviews
				.FirstOrDefaultAsync(p => p.LeaderReviewId == id)
				?? throw new KeyNotFoundException($"Leader review with ID {id} not found.");
		}

		
	}
}
