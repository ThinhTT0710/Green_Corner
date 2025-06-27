using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;

namespace GreenCorner.MVC.Services
{
    public class RewardService : IRewardService
    {
        private readonly IBaseService _baseService;
        public RewardService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO?> GetAllReward()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.RewardAPIBase + "/api/Voucher"
            });
        }
    }
}

