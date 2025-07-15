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

        [HttpGet("get-all")]
        public async Task<ResponseDTO> GetAll()
        {
            try
            {
                var allPointtransaction = await _pointTransactionService.GetAllPointTransaction();
                _responseDTO.Result = allPointtransaction;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("get-by-user/{userId}")]
        public async Task<ResponseDTO> GetUserPointTransaction(string userId)
        {
            try
            {
                var pointTransactions = await _pointTransactionService.GetByUserId(userId);
                _responseDTO.Result = pointTransactions;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
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

		// Giao dịch theo loại (Kiếm / Đổi)
		[HttpPost("Transaction")]
		public async Task<ResponseDTO> TransactionPoints([FromBody] PointTransactionDTO request)
		{
			try
			{
				await _pointTransactionService.TransactionPoints(request.UserId, request.Points, request.Type, request.CleanEventId);
				_responseDTO.Message = $"Giao dịch {request.Type} điểm thành công.";
				return _responseDTO;
			}
            catch (InvalidOperationException ex)
            {
                _responseDTO.Message = "Bạn không đủ điểm!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
            catch (Exception ex)
			{
				_responseDTO.Message = "Đã xảy ra lỗi!";
				_responseDTO.IsSuccess = false;
				return _responseDTO;
			}
		}

		[HttpGet("reward/{userId}")]
		public async Task<ResponseDTO> GetRewardPoint(string userId)
		{
			try
			{
				var rewardPoint = await _pointTransactionService.GetRewardPointByUserIdAsync(userId);
				_responseDTO.Result = rewardPoint;
				return _responseDTO;
			}
			catch (Exception ex)
			{
				_responseDTO.Message = "Lấy thông tin thất bại!";
				_responseDTO.IsSuccess = false;
				return _responseDTO;
			}
		}

		[HttpPost("add-transaction")]
		public async Task<ResponseDTO> AddTransaction([FromBody] PointTransactionDTO dto)
		{
			try
			{
				await _pointTransactionService.AddTransactionAsync(dto);
				_responseDTO.Message = "Giao dịch đã được thêm.";
				return _responseDTO;
			}
			catch (Exception ex)
			{
				_responseDTO.Message = "Thêm thất bại!";
				_responseDTO.IsSuccess = false;
				return _responseDTO;
			}
		}

		[HttpPut("update-reward")]
		public async Task<ResponseDTO> UpdateRewardPoint([FromBody] RewardPointDTO dto)
		{
			try
			{
				await _pointTransactionService.UpdateRewardPointAsync(dto);
				_responseDTO.Message = "Điểm thưởng đã được cập nhật.";
				return _responseDTO;
			}
			catch (Exception ex)
			{
				_responseDTO.Message = "Cập nhật điểm thất bại!";
				_responseDTO.IsSuccess = false;
				return _responseDTO;
			}
		}

        [HttpGet("rewardpointshistory")]
        public async Task<ResponseDTO> GetRewardPointHistory()
        {
            try
            {
                var rewardPoint = await _pointTransactionService.GetPointsAwardHistoryAsync();
                _responseDTO.Result = rewardPoint;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Lấy lịch sử trao điểm thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("has-received/{userId}/{eventId}")]
        public async Task<ResponseDTO> HasReceivedReward(string userId, int eventId)
        {
            try
            {
                bool hasReceived = await _pointTransactionService.HasReceivedReward(userId, eventId);
                _responseDTO.Result = hasReceived;
                _responseDTO.IsSuccess = true;
                _responseDTO.Message = hasReceived
                    ? "Người dùng đã nhận điểm cho sự kiện này."
                    : "Người dùng chưa nhận điểm cho sự kiện này.";
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = "Đã xảy ra lỗi khi kiểm tra!";
                return _responseDTO;
            }
        }

    }
}
