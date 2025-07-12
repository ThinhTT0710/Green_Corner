using GreenCorner.RewardAPI.Data;
using GreenCorner.RewardAPI.Models;
using GreenCorner.RewardAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.RewardAPI.Repositories
{
    public class RewardRedemptionHistoryRepository : IRewardRedemptionHistoryRepository

    {
        private readonly GreenCornerRewardContext _context;

        public RewardRedemptionHistoryRepository(GreenCornerRewardContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PointTransaction>> GetRewardRedemptionHistory(string userId)
        {
            var list = await _context.PointTransactions
                .Where(p => p.UserId == userId)
                .ToListAsync();

            if (list == null || list.Count == 0)
                throw new KeyNotFoundException($"No transactions found for user {userId}.");

            return list;
        }

    }
}
