using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;
using Newtonsoft.Json;
using System.Text;

namespace GreenCorner.MVC.Services
{
    public class RewardPointService : IRewardPointService
    {
        private readonly IBaseService _baseService;

        public RewardPointService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO?> GetRewardPoints()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.RewardAPIBase + "/api/RewardPoint"
            });
        }

        public async Task<ResponseDTO?> AwardPointsToUser(string userId, int points)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Url = SD.RewardAPIBase + "/api/RewardPoint/" + userId + "/" + points

            });
        }

        public async Task<ResponseDTO?> GetUserTotalPoints(string userId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.RewardAPIBase + "/api/RewardPoint/" + userId
            });
        }
    }

}