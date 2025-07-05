using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;

namespace GreenCorner.MVC.Services
{
    public class WishListService : IWishListService
    {
        private readonly IBaseService _baseService;

        public WishListService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> AddToWishList(WishListDTO wishListDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = wishListDTO,
                Url = SD.EcommerceAPIBase + "/api/wishlist"
            });
        }

        public async Task<ResponseDTO?> Delete(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.DELETE,
                Url = SD.EcommerceAPIBase + "/api/wishlist/" + id
            });
        }

        public async Task<ResponseDTO?> GetUserWishList(string userId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EcommerceAPIBase + "/api/wishlist/get-user-wishlist/" + userId
            });
        }
    }
}
