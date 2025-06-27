using GreenCorner.RewardAPI.Models;
using GreenCorner.RewardAPI.Models.DTO;

namespace GreenCorner.RewardAPI.Services.Interface
{
    public interface IVoucherService
    {
        Task<IEnumerable<VoucherDTO>> GetAllRewards();

        Task<VoucherDTO> GetRewardDetail(int voucherId);
        Task CreateVoucher(VoucherDTO voucher);
        Task UpdateVoucher(VoucherDTO voucher);
        Task DeleteVoucher(int voucherId);
        Task<IEnumerable<VoucherDTO>> GetAllVouchers();


    }
}
