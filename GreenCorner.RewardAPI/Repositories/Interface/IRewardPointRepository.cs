using GreenCorner.RewardAPI.Models;
using GreenCorner.RewardAPI.Models.DTO;

namespace GreenCorner.RewardAPI.Repositories.Interface
{
    public interface IRewardPointRepository
    {
        Task<IEnumerable<RewardPoint>> GetRewardPoints();
        Task AwardPointsToUser(string userId, int points);
        Task<RewardPoint> GetUserTotalPoints(string userId);
    }
}
