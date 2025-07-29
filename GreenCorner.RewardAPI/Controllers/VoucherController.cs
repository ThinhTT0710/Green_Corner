using GreenCorner.RewardAPI.Models.DTO;
using GreenCorner.RewardAPI.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.RewardAPI.Controllers
{
    [Route("api/Voucher")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;
        private readonly ResponseDTO _responseDTO;

        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
            _responseDTO = new ResponseDTO();
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN, EVENTSTAFF")]
        public async Task<ResponseDTO> GetAllVouchers()
        {
            try
            {
                var vouchers = await _voucherService.GetAllVouchers();
                _responseDTO.Result = vouchers;
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
        public async Task<ResponseDTO> GetVoucherById(int id)
        {
            try
            {
                var voucher = await _voucherService.GetRewardDetail(id);
                _responseDTO.Result = voucher;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Lấy thông tin thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
        

        [HttpPost]
        public async Task<ResponseDTO> CreateVoucher([FromBody] VoucherDTO voucher)
        {
            try
            {
                await _voucherService.CreateVoucher(voucher);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Tạo Voucher thất bại.";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPut]
        public async Task<ResponseDTO> UpdateVoucher([FromBody] VoucherDTO voucher)
        {
            try
            {
                await _voucherService.UpdateVoucher(voucher);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Cập nhật Voucher thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

      

        [HttpDelete("{id}")]
        public async Task<ResponseDTO> DeleteVoucher(int id)
        {
            try
            {
                await _voucherService.DeleteVoucher(id);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Xóa voucher thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("getAllRewards")]
        public async Task<ResponseDTO> GetAllRewards()
        {
            try
            {
                var vouchers = await _voucherService.GetAllVouchers();
                _responseDTO.Result = vouchers;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("top10voucher")]
        public async Task<ResponseDTO> GetTop10Vouchers()
        {
            try
            {
                var vouchers = await _voucherService.GetTop10ValidVouchersAsync();
                _responseDTO.Result = vouchers;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPost("redeem/{id}")]
        public async Task<ResponseDTO> RedeemVoucher(int id)
        {
            try
            {
                var result = await _voucherService.RedeemVoucherAsync(id);

                if (!result)
                {
                    _responseDTO.IsSuccess = false;
                    _responseDTO.Message = "Voucher không khả dụng hoặc đã hết.";
                }
                else
                {
                    _responseDTO.Message = "Đổi voucher thành công!";
                }

                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = "Đổi voucher thất bại!";
                return _responseDTO;
            }
        }

        [HttpDelete("cleanup")]
        public async Task<ResponseDTO> CleanUpExpiredOrEmptyVouchers()
        {
            try
            {
                await _voucherService.CleanUpExpiredOrEmptyVouchersAsync();
                _responseDTO.Message = "Dọn dẹp thành công các voucher hết hạn hoặc đã hết số lượng.";
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = "Dọn dẹp thất bại: " + ex.Message;
                return _responseDTO;
            }
        }
    }
}
