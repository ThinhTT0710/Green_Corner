using GreenCorner.EventAPI.Models.DTO;
using GreenCorner.EventAPI.Services;
using GreenCorner.EventAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.EventAPI.Controllers
{
    [Route("api/LeaderReview")]
    [ApiController]
    public class LeaderReviewController : ControllerBase
    {
        private readonly ILeaderReviewService _leaderReviewService;
        private readonly ResponseDTO _responseDTO;
        public LeaderReviewController(ILeaderReviewService leaderReviewService)
        {

            _leaderReviewService = leaderReviewService;
            _responseDTO = new ResponseDTO();
        }
        [HttpPost]
        public async Task<ResponseDTO> LeaderReview([FromBody] LeaderReviewDTO leaderReview)
        {
            try
            {
                await _leaderReviewService.LeaderReview(leaderReview);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
		[HttpGet("get-reviewid/{id}")]
		public async Task<ResponseDTO> GetLeaderReviewById(int id)
		{
			try
			{
				var eventReview = await _leaderReviewService.GetLeaderReviewById(id);
				_responseDTO.Result = eventReview;
				return _responseDTO;
			}
			catch (Exception ex)
			{
				_responseDTO.Message = ex.Message;
				_responseDTO.IsSuccess = false;
				return _responseDTO;
			}
		}
		[HttpPut]
        public async Task<ResponseDTO> EditLeaderReview([FromBody] LeaderReviewDTO leaderReview)
        {
            try
            {
                await _leaderReviewService.EditLeaderReview(leaderReview);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
        [HttpDelete("{id}")]
        public async Task<ResponseDTO> DeleteLeaderReview(int id)
        {
            try
            {
                await _leaderReviewService.DeleteLeaderReview(id);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
        [HttpGet("{id}")]
        public async Task<ResponseDTO> ViewLeaderReviewHistory(string id)
        {
            try
            {
                var leaderReviewHistory = await _leaderReviewService.ViewLeaderReviewHistory(id);
                _responseDTO.Result = leaderReviewHistory;
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
