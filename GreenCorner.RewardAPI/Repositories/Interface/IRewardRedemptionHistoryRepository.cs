using GreenCorner.RewardAPI.Models;

namespace GreenCorner.RewardAPI.Repositories.Interface
{
    public interface IRewardRedemptionHistoryRepository
    {
        Task<IEnumerable<UserVoucherRedemption>> GetRewardRedemptionHistory(string userId);
        Task SaveRedemptionAsync(string userId, int voucherId);
    }
}
