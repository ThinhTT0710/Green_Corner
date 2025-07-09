using GreenCorner.RewardAPI.Models.DTO;

namespace GreenCorner.RewardAPI.Services.Interface
{
    public interface IRewardPointService
    {
        Task<IEnumerable<RewardPointDTO>> GetRewardPoints();
        Task AwardPointsToUser(string userId, int points); 
        Task<RewardPointDTO> GetUserTotalPoints(string userId);

    }
}
