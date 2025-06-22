using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface ICartService
    {
        Task<ResponseDTO?> DeleteItemInCart(int cartId);
        Task<ResponseDTO?> AddToCart(CartDTO cartDto);
        Task<ResponseDTO?> GetUserCart(string userId);
        Task<ResponseDTO?> UpdateCart(CartDTO cartDto);
        Task<ResponseDTO?> DeleteUserCart(string userId);

	}
}
