using GreenCorner.EcommerceAPI.Data;
using GreenCorner.EcommerceAPI.Models;
using GreenCorner.EcommerceAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.EcommerceAPI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly GreenCornerEcommerceContext _context;

        public CartRepository(GreenCornerEcommerceContext context)
        {
            _context = context;
        }

        public async Task AddToCart(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCart(int id)
        { 
            var cart = await GetById(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Cart with ID {id} not found.");
            }
        }

		public async Task DeleteUserCart(string userId)
		{
			var userCarts = await _context.Carts
								  .Where(c => c.UserId == userId)
								  .ToListAsync();

			if (userCarts == null || userCarts.Count == 0)
			{
				throw new Exception($"No cart found for User ID {userId}.");
			}

			_context.Carts.RemoveRange(userCarts);
			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Cart>> GetAll()
        {
            return await _context.Carts.ToListAsync();
        }

        public async Task<Cart> GetById(int id)
        {
            return await _context.Carts.FirstOrDefaultAsync(c => c.CartId == id);
        }

        public async Task<IEnumerable<Cart>> GetByUserId(string userId)
        {
            return await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<Cart> GetCartItem(string userId, int productId)
        {
            return await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
        }

        public async Task UpdateCart(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }
    }
}
