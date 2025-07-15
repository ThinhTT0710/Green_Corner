using GreenCorner.RewardAPI.Models.DTO;
using GreenCorner.RewardAPI.Services;
using GreenCorner.RewardAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;



namespace GreenCorner.RewardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RewardPointController : ControllerBase
    {
        private readonly IRewardPointService _rewardPointService;
        private readonly ResponseDTO _responseDTO;

        public RewardPointController(IRewardPointService rewardPointService)
        {
            _rewardPointService = rewardPointService;
            _responseDTO = new ResponseDTO();
        }

        [HttpGet]
        public async Task<ResponseDTO> GetRewardPoints()
        {
            try
            {
                var rewardPoints = await _rewardPointService.GetRewardPoints();
                _responseDTO.Result = rewardPoints;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Lấy điểm thưởng thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
        [HttpPost("{userId}/{points}")]
        public async Task<ResponseDTO> AwardPointsToUser(string userId, int points)
        {
            try
            {
                await _rewardPointService.AwardPointsToUser(userId,points);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Trao điểm thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("{id}")]
        public async Task<ResponseDTO> GetUserTotalPoints(string id)
        {
            try
            {
                var totalPoints = await _rewardPointService.GetUserTotalPoints(id);
                _responseDTO.Result = totalPoints;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Lấy điểm thưởng thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
    }

}
