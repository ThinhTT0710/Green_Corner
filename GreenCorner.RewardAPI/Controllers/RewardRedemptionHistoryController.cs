using GreenCorner.RewardAPI.Models.DTO;
using GreenCorner.RewardAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.RewardAPI.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class RewardRedemptionHistoryController : Controller
    {
        private readonly IRewardRedemptionHistoryService _rewardRedemptionHistoryService;
        private readonly ResponseDTO _responseDTO;
        public RewardRedemptionHistoryController(IRewardRedemptionHistoryService rewardRedemptionHistoryService)
        {
            _rewardRedemptionHistoryService = rewardRedemptionHistoryService;
            _responseDTO = new ResponseDTO();
        }
        [HttpGet()]
        public async Task<ResponseDTO> GetRewardRedemptionHistory(string userId)
        {
            try
            {
                var pointTransaction = await _rewardRedemptionHistoryService.GetRewardRedemptionHistory(userId);
                _responseDTO.Result = pointTransaction;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
    }
}
