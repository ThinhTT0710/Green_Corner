using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IRewardRedemptionHistoryService
    {
        Task<ResponseDTO?> GetRewardRedemptionHistory(string userId);
    }
}
