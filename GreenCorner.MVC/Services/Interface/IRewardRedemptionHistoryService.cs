using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IRewardRedemptionHistoryService
    {
        Task<ResponseDTO?> GetRewardRedemptionHistory(string userId);
        Task<ResponseDTO?> SaveRedemptionAsync(string userId, int voucherId);
        Task<ResponseDTO?> GetUserRewardRedemption();
        Task<ResponseDTO?> MarkAsUsedAsync(int userVoucherId);

    }
}
