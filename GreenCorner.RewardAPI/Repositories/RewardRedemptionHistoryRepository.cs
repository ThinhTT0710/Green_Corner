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
        public async Task<IEnumerable<UserVoucherRedemption>> GetRewardRedemptionHistory(string userId)
        {
            var list = await _context.UserVoucherRedemptions 
                .Where(p => p.UserId == userId)
                .ToListAsync();

            if (list == null || list.Count == 0)
                throw new KeyNotFoundException($"No transactions found for user {userId}.");

            return list;
        }

        public async Task SaveRedemptionAsync(string userId, int voucherId)
        {
            var voucher = await _context.Vouchers.FindAsync(voucherId);
            if (voucher == null)
                throw new Exception("Voucher không tồn tại.");

            var redemption = new UserVoucherRedemption
            {
                UserId = userId,
                VoucherId = voucherId,
                RedeemedAt = DateTime.UtcNow
            };

            await _context.UserVoucherRedemptions.AddAsync(redemption);
            await _context.SaveChangesAsync();
        }
    }
}
