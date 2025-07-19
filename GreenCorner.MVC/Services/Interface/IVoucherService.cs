using GreenCorner.MVC.Models;
using GreenCorner.MVC.Utility;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IVoucherService
    {
        Task<ResponseDTO?> GetAllVoucher();
        Task<ResponseDTO?> GetVoucherById(int id);
        Task<ResponseDTO?> AddVoucher(VoucherDTO voucherDto);
        Task<ResponseDTO?> UpdateVoucher(VoucherDTO voucherDto);
        Task<ResponseDTO?> DeleteVoucher(int id);
        Task<ResponseDTO?> GetTop10ValidVouchersAsync();
        Task<ResponseDTO?> RedeemVoucher(int voucherId);
    }

}
