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

        public async Task<IEnumerable<PointTransaction>> GetAllPointTransaction()
        {
            return await _context.PointTransactions.ToListAsync();
        }

        public async Task<IEnumerable<PointTransaction>> GetByUserId(string userId)
        {
            return await _context.PointTransactions
                .Where(t => t.UserId == userId)
                .ToListAsync();
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
              ?? throw new Exception($"User with ID {userId} not found.");
        }

		public async Task<IEnumerable<PointTransaction>> GetRewardPointByUserIdAsync(string userId)
		{
			return await _context.PointTransactions
				.Where(rp => rp.UserId == userId)
				.OrderByDescending(rp => rp.CreatedAt) // Sắp xếp nếu muốn
				.ToListAsync();
		}


		public async Task AddTransactionAsync(PointTransaction transaction)
		{
			transaction.CreatedAt = DateTime.UtcNow;
			await _context.PointTransactions.AddAsync(transaction);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateRewardPointAsync(RewardPoint rewardPoint)
		{
			_context.RewardPoints.Update(rewardPoint);
			await _context.SaveChangesAsync();
		}

		public async Task TransactionPoints(string userId, int points, string type, int? eventId = null)
		{
			if (type != "Thưởng" && type != "Đổi")
				throw new ArgumentException("Type must be 'Kiếm' or 'Đổi'.");

			var rewardPoint = await _context.RewardPoints.FirstOrDefaultAsync(rp => rp.UserId == userId);

			if (rewardPoint == null)
			{
				rewardPoint = new RewardPoint
				{
					UserId = userId,
					TotalPoints = 0
				};
				_context.RewardPoints.Add(rewardPoint);
			}

			if (type == "Thưởng")
			{
				rewardPoint.TotalPoints += points;
			}
			else if (type == "Đổi")
			{
                if (rewardPoint.TotalPoints < points)
                {
                    throw new InvalidOperationException("Bạn không có đủ điểm để thực hiện giao dịch này.");
                }
                rewardPoint.TotalPoints -= points;
			}

			var transaction = new PointTransaction
			{
				UserId = userId,
				Points = type == "Thưởng" ? points : -points,
				Type = type,
				CreatedAt = DateTime.UtcNow,
                CleanEventId = eventId ?? 0
            };

			await UpdateRewardPointAsync(rewardPoint);
			await _context.PointTransactions.AddAsync(transaction);
			await _context.SaveChangesAsync();			
		}

        public async Task<IEnumerable<PointTransaction>> GetPointsAwardHistoryAsync()
        {
            return await _context.PointTransactions
						.Where(t => t.Type == "Thưởng")
						.ToListAsync();
        }

        public async Task<bool> HasReceivedReward(string userId, int eventId)
        {
            return await _context.PointTransactions.AnyAsync(pt =>
                pt.UserId == userId &&
                pt.Type == "Thưởng" &&
                pt.CleanEventId == eventId);
        }
    }
}
