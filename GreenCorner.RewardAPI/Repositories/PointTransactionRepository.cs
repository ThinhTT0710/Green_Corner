using AutoMapper;
using GreenCorner.RewardAPI.Data;
using GreenCorner.RewardAPI.Models;
using GreenCorner.RewardAPI.Models.DTO;
using GreenCorner.RewardAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.RewardAPI.Repositories
{
    public class PointTransactionRepository : IPointTransactionRepository
    {
        private readonly GreenCornerRewardContext _context;

        public PointTransactionRepository(GreenCornerRewardContext context)
        {
            _context = context;
        }

        public async Task TransactionPoint(string userId, int points)
        {
            var userTransaction = await _context.PointTransactions.FirstOrDefaultAsync(rp => rp.UserId == userId);
            var user = await _context.RewardPoints.FirstOrDefaultAsync(rp => rp.UserId == userId);
            user.TotalPoints-=points;
            await _context.SaveChangesAsync();
        }

        public async Task<PointTransaction> GetPointTransaction(string userId)
        {
            return await _context.PointTransactions
              .FirstOrDefaultAsync(p => p.UserId == userId)
              ?? throw new KeyNotFoundException($"User with ID {userId} not found.");
        }
    }
}
