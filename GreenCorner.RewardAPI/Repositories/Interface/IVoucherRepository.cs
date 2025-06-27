using GreenCorner.RewardAPI.Models;
using GreenCorner.RewardAPI.Models.DTO;

namespace GreenCorner.RewardAPI.Repositories.Interface
{
    public interface IVoucherRepository
    {
        Task<IEnumerable<Voucher>> GetAllRewards();
        Task<Voucher> GetRewardDetail(int voucherId);
        Task CreateVoucher(Voucher voucher);
        Task  UpdateVoucher(Voucher voucher);
        Task DeleteVoucher(int voucherId);
        Task<IEnumerable<Voucher>> GetAllVouchers();



    }

}

