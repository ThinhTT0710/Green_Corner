using GreenCorner.EcommerceAPI.Models.DTO;

namespace GreenCorner.EcommerceAPI.Services.Interface
{
    public interface ICartService
    {
        Task<IEnumerable<CartDTO>> GetAll();
        Task<CartDTO> GetById(int id);
        Task AddToCart(CartDTO item);
        Task UpdateCart(CartDTO item);
        Task DeleteCart(int id);
		Task DeleteUserCart(string userId);
		Task<IEnumerable<CartDTO>> GetByUserId(string userId);
    }
}
