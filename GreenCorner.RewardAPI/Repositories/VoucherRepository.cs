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
            var today = DateTime.Today;

            return await _context.Vouchers
                .Where(v => v.IsActive == true)
                .OrderBy(v => v.ExpirationDate < today) 
                .ThenBy(v => v.ExpirationDate)          
                .ToListAsync();
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
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVoucher(int voucherId)
        {
            var voucher = await _context.Vouchers.FindAsync(voucherId);
            if (voucher == null)
            {
                throw new KeyNotFoundException($"Voucher with ID {voucherId} not found.");
            }

            voucher.IsActive = false; // Cập nhật trạng thái không còn hiệu lực

            _context.Vouchers.Update(voucher); // Cập nhật lại vào DbContext
            await _context.SaveChangesAsync(); // Lưu thay đổi
        }

        public async Task<IEnumerable<Voucher>> GetTop10ValidVouchersAsync()
        {
            var now = DateTime.Today;

            var validVouchers = await _context.Vouchers
                .Where(v => v.ExpirationDate >= now && v.IsActive)
                .OrderBy(v => v.ExpirationDate)
                .Take(10)
                .ToListAsync();

            if (!validVouchers.Any())
            {
                validVouchers = await _context.Vouchers
                    .OrderBy(v => Guid.NewGuid())
                    .Take(10)
                    .ToListAsync();
            }

            return validVouchers;
        }

        public async Task CleanUpExpiredOrEmptyVouchersAsync()
        {
            var today = DateTime.Today;

            var vouchersToDeactivate = await _context.Vouchers
                .Where(v =>
                    v.IsActive && 
                    (
                        (v.ExpirationDate != null && EF.Functions.DateDiffDay(v.ExpirationDate, today) > 2) ||
                        v.Quantity == 0
                    ))
                .ToListAsync();

            if (vouchersToDeactivate.Any())
            {
                foreach (var voucher in vouchersToDeactivate)
                {
                    voucher.IsActive = false;
                }

                await _context.SaveChangesAsync();
            }
        }

    }
}
