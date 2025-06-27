using GreenCorner.RewardAPI.Models.DTO;
using GreenCorner.RewardAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.RewardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PointTransactionController : ControllerBase
    {

        private readonly IPointTransactionService _pointTransactionService;
        private readonly ResponseDTO _responseDTO;
        public PointTransactionController(IPointTransactionService  pointTransactionService)
        {
            _pointTransactionService = pointTransactionService;
            _responseDTO = new ResponseDTO();
        }
        [HttpPost("{userId},{points}")]
        public async Task<ResponseDTO> TransactionPoint(string userId, int points)
        {
            try
            {
                await _pointTransactionService.TransactionPoint(userId, points);
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
        public async Task<ResponseDTO> GetPointTransaction(string id)
        {
            try
            {
                var totalPoints = await _pointTransactionService.GetPointTransaction(id);
                _responseDTO.Result = totalPoints;
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
