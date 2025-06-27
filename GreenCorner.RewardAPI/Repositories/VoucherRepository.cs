using GreenCorner.RewardAPI.Data;
using GreenCorner.RewardAPI.Models;
using GreenCorner.RewardAPI.Models.DTO;
using GreenCorner.RewardAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.RewardAPI.Repositories
{
    public class VoucherRepository : IVoucherRepository

    {
        private readonly GreenCornerRewardContext _context;

        public bool IsDeleted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public VoucherRepository(GreenCornerRewardContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Voucher>> GetAllRewards()
        {
            return await _context.Vouchers.ToListAsync();
        }
        public async Task<IEnumerable<Voucher>> GetAllVouchers()
        {
            return await _context.Vouchers.ToListAsync();
        }

        public async Task<Voucher> GetRewardDetail(int voucherId)
        {
            return await _context.Vouchers
               .FirstOrDefaultAsync(p => p.VoucherId == voucherId)
               ?? throw new KeyNotFoundException($"Voucher with ID {voucherId} not found.");
        }
        public async Task CreateVoucher(Voucher voucher)
        {


         
            voucher.StartDate = DateTime.Now;
            await _context.Vouchers.AddAsync(voucher);
            await _context.SaveChangesAsync();
        
        }
       

        public async Task UpdateVoucher(Voucher voucher)
        {
            var product = await _context.Vouchers.FindAsync(voucher.VoucherId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Voucher with ID {voucher.VoucherId} not found.");
            }
            _context.Entry(product).CurrentValues.SetValues(voucher);
            product.StartDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVoucher(int voucherId)
        {
            var product = await _context.Vouchers.FindAsync(voucherId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Voucher with ID {voucherId} not found.");
            }
            //product.IsDeleted = true;
            _context.Vouchers.Remove(product);
            await _context.SaveChangesAsync();
        }

       
    }
}
