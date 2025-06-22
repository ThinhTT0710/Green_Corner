using GreenCorner.EcommerceAPI.Models;

namespace GreenCorner.EcommerceAPI.Repositories.Interface
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetAll();
        Task<Cart> GetById(int id);
        Task AddToCart(Cart cart);
        Task UpdateCart(Cart cart);
        Task DeleteCart(int id);
		Task DeleteUserCart(string UserId);
		Task<IEnumerable<Cart>> GetByUserId(string userId);
        Task<Cart> GetCartItem(string userId, int productId);
    }
}
