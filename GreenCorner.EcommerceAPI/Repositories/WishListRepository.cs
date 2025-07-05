using GreenCorner.EcommerceAPI.Data;
using GreenCorner.EcommerceAPI.Models;
using GreenCorner.EcommerceAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.EcommerceAPI.Repositories
{
    public class WishListRepository : IWishListRepository
    {
        private readonly GreenCornerEcommerceContext _context;
        public WishListRepository(GreenCornerEcommerceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WishList>> GetAll()
        {
            return await _context.WishLists.ToListAsync();
        }

        public async Task<WishList> GetById(int id)
        {
            return await _context.WishLists.FirstOrDefaultAsync(w => w.WishListId == id);
        }

        public async Task Add(WishList item)
        {
                await _context.WishLists.AddAsync(item);
                await _context.SaveChangesAsync();
        }

        public async Task Update(WishList item)
        {
            var WishList = await GetById(item.WishListId);
            if (WishList == null)
            {
                throw new Exception("WishList not found");
            }
            _context.WishLists.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {

            var item = await GetById(id);
            if (item == null)
            {
                throw new Exception("WishList not found");
            }
            _context.WishLists.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<WishList>> GetByUserId(string userID)
        {
            return await _context.WishLists
                .Include(c => c.Product)
                .Where(c => c.UserId == userID)
                .ToListAsync(); ;
        }
    }
}
