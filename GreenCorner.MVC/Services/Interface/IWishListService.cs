using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IWishListService
    {
        Task<ResponseDTO?> AddToWishList(WishListDTO wishListDTO);
        Task<ResponseDTO?> GetUserWishList(string userId);
        Task<ResponseDTO?> Delete(int id);
        Task<ResponseDTO?> DeleteByUserId(string userId, int productId);
    }
}
