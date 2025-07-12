using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;

namespace GreenCorner.MVC.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IBaseService _baseService;
        public VoucherService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> GetAllVoucher()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.RewardAPIBase + "/api/Voucher"
            });
        }

        public async Task<ResponseDTO?> GetVoucherById(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.RewardAPIBase + "/api/Voucher/"+id
            });
        }

        public async Task<ResponseDTO?> AddVoucher(VoucherDTO voucherDto)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Url = SD.RewardAPIBase + "/api/Voucher",
                Data = voucherDto
            });
        }

        public async Task<ResponseDTO?> UpdateVoucher(VoucherDTO voucherDto)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.PUT,
                Url = SD.RewardAPIBase + "/api/Voucher",
                Data = voucherDto
            });
        }

        public async Task<ResponseDTO?> DeleteVoucher(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.DELETE,
                Url = SD.RewardAPIBase + "/api/Voucher/"+id
            });
        }
    }

}
