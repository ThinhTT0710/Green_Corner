using GreenCorner.RewardAPI.Models;
using GreenCorner.RewardAPI.Models.DTO;

namespace GreenCorner.RewardAPI.Services.Interface
{
    public interface IRewardRedemptionHistoryService
    {
        Task<IEnumerable<PointTransactionDTO>> GetRewardRedemptionHistory(string userId);
    }
}
