using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;

namespace GreenCorner.MVC.Services
{
    public class RewardRedemptionHistoryService : IRewardRedemptionHistoryService
    {
        private readonly IBaseService _baseService;

        public RewardRedemptionHistoryService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO?> GetRewardRedemptionHistory(string userId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.RewardAPIBase + $"/api/RewardRedemptionHistory?userId={userId}"
            });
        }

        public async Task<ResponseDTO?> GetUserRewardRedemption()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.RewardAPIBase + $"/api/RewardRedemptionHistory/userredemp"
            });
        }

        public async Task<ResponseDTO?> SaveRedemptionAsync(string userId, int voucherId)
        {
            var dto = new UserVoucherRedemptionDTO
            {
                UserId = userId,
                VoucherId = voucherId
            };

            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Url = SD.RewardAPIBase + "/api/RewardRedemptionHistory/saveredemp",
                Data = dto
            });
        }

        public async Task<ResponseDTO?> MarkAsUsedAsync(int userVoucherId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.PUT,
                Url = SD.RewardAPIBase + $"/api/RewardRedemptionHistory/markused/{userVoucherId}"
            });
        }

    }
}
