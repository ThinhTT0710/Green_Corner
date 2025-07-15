using GreenCorner.RewardAPI.Models;

namespace GreenCorner.RewardAPI.Repositories.Interface
{
    public interface IPointTransactionRepository
    {
        Task TransactionPoint(string userId, int points);
        Task<PointTransaction> GetPointTransaction(string userId);

		Task TransactionPoints(string userId, int points, string type, int? eventId);
		Task<IEnumerable<PointTransaction>> GetRewardPointByUserIdAsync(string userId);
		Task AddTransactionAsync(PointTransaction transaction);
		Task UpdateRewardPointAsync(RewardPoint rewardPoint);
        Task<IEnumerable<PointTransaction>> GetPointsAwardHistoryAsync();
        Task<bool> HasReceivedReward(string userId, int eventId);
    }

}


