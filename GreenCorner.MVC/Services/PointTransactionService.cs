using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner.MVC.Services
{
    public class PointTransactionService : IPointTransactionService
    {
        private readonly IBaseService _baseService;

        public PointTransactionService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> GetAllPointTransactions()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.RewardAPIBase + "/api/PointTransaction/get-all"
            });
        }

        public async Task<ResponseDTO?> GetUserTransactions(string userId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.RewardAPIBase + "/api/PointTransaction/get-by-user/" + userId
            });
        }

        public async Task<ResponseDTO?> ExchangePoints(string userId, int exchangePoint)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Url = SD.RewardAPIBase + $"/api/PointTransaction/{userId},{exchangePoint}",
            });
        }

 
        public async Task<ResponseDTO?> GetExchangeTransactions(string userId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.RewardAPIBase + $"/api/ExchangePoint/transactions/{userId}"
            });
        }


        public async Task<ResponseDTO?> GetUserRewardPoints(string userId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.RewardAPIBase + $"/api/RewardPoint/{userId}"
            });
        }


        public async Task<ResponseDTO?> AwardRewardPoints(RewardPointDTO rewardPointDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Url = SD.RewardAPIBase + "/api/RewardPoint/award",
                Data = rewardPointDTO
            });
        }


        public async Task<ResponseDTO?> GetTotalRewardPoints(string userId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.RewardAPIBase + $"/api/RewardPoint/total/{userId}"
            });
        }

		public async Task<ResponseDTO?> TransactionPoints(PointTransactionDTO dto)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				APIType = SD.APIType.POST,
				Url = SD.RewardAPIBase + "/api/PointTransaction/Transaction",
				Data = dto
			});
		}

		public async Task<ResponseDTO?> GetUserPointTransactions(string userId)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				APIType = SD.APIType.GET,
				Url = SD.RewardAPIBase + $"/api/PointTransaction/reward/{userId}"
			});
		}

        public async Task<ResponseDTO?> GetPointsAwardHistoryAsync()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.RewardAPIBase + $"/api/PointTransaction/rewardpointshistory"
            });
        }

        public async Task<ResponseDTO?> HasReceivedReward(string userId, int eventId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.RewardAPIBase + $"/api/PointTransaction/has-received/{userId}/{eventId}"
            });
        }

    }

}