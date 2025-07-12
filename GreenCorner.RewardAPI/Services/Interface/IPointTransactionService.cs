using GreenCorner.RewardAPI.Models;
using GreenCorner.RewardAPI.Models.DTO;

namespace GreenCorner.RewardAPI.Services.Interface
{
    public interface IPointTransactionService
    {
        Task TransactionPoint(string userId, int points);
        Task<PointTransactionDTO> GetPointTransaction(string userId);

		Task TransactionPoints(string userId, int points, string type);
		Task<IEnumerable<PointTransactionDTO>> GetRewardPointByUserIdAsync(string userId);
		Task AddTransactionAsync(PointTransactionDTO transaction);
		Task UpdateRewardPointAsync(RewardPointDTO rewardPoint);
        Task<IEnumerable<PointTransactionDTO>> GetPointsAwardHistoryAsync();
    }

}
