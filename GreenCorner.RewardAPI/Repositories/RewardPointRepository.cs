using GreenCorner.RewardAPI.Data;
using GreenCorner.RewardAPI.Models;
using GreenCorner.RewardAPI.Models.DTO;
using GreenCorner.RewardAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.RewardAPI.Repositories
{
    public class RewardPointRepository : IRewardPointRepository
    {
        private readonly GreenCornerRewardContext _context;

        public RewardPointRepository(GreenCornerRewardContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RewardPoint>> GetRewardPoints()
        {
            return await _context.RewardPoints.ToListAsync();
        }
        public async Task AwardPointsToUser(string userId, int points)
        {
            var userReward = await _context.RewardPoints.FirstOrDefaultAsync(rp => rp.UserId == userId);
            userReward.TotalPoints += points; 
            await _context.SaveChangesAsync();
        }

        public async Task<RewardPoint> GetUserTotalPoints(string userId)
        {
            return await _context.RewardPoints
              .FirstOrDefaultAsync(p => p.UserId == userId)
              ?? throw new KeyNotFoundException($"User with ID {userId} not found.");
        }

    }

}
