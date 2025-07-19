using GreenCorner.RewardAPI.Models;
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
                _responseDTO.Message = "Lấy lịch sử đổi điểm thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
        [HttpPost("saveredemp")]
        public async Task<ResponseDTO> SaveRedemption([FromBody] UserVoucherRedemptionDTO dto)
        {
            try
            {
                await _rewardRedemptionHistoryService.SaveRedemptionAsync(dto.UserId, dto.VoucherId);
                _responseDTO.Message = "Lưu lịch sử đổi điểm thành công.";
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Lưu lịch sử đổi điểm thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("userredemp")]
        public async Task<ResponseDTO> GetUserRewardRedemption()
        {
            try
            {
                var pointTransaction = await _rewardRedemptionHistoryService.GetDistinctUserIdsRedeemedAsync();
                _responseDTO.Result = pointTransaction;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Lấy lịch sử đổi điểm thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPut("markused/{userVoucherId}")]
        public async Task<ResponseDTO> MarkAsUsed(int userVoucherId)
        {
            try
            {
                var result = await _rewardRedemptionHistoryService.UpdateIsUsedAsync(userVoucherId);
                _responseDTO.Result = result;
                _responseDTO.Message = "Sử dụng Voucher thành công.";
            }
            catch (KeyNotFoundException ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
            }
            catch (InvalidOperationException ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
            }
            catch (Exception)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = "Sử dụng Voucher thất bại!";
            }

            return _responseDTO;
        }

    }
}
