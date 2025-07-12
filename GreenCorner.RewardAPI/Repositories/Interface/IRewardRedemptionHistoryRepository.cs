using GreenCorner.RewardAPI.Models;

namespace GreenCorner.RewardAPI.Repositories.Interface
{
    public interface IRewardRedemptionHistoryRepository
    {
        Task<IEnumerable<PointTransaction>> GetRewardRedemptionHistory(string userId);
    }
}
