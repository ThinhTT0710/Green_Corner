using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;

namespace GreenCorner.MVC.Services
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;

        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> AddToCart(CartDTO cartDto)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = cartDto,
                Url = SD.EcommerceAPIBase + "/api/cart"
            });
        }

        public async Task<ResponseDTO?> UpdateCart(CartDTO cartDto)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.PUT,
                Data = cartDto,
                Url = SD.EcommerceAPIBase + "/api/cart"
            });
        }

        public async Task<ResponseDTO?> GetUserCart(string userId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EcommerceAPIBase + "/api/cart/get-user-cart/" + userId
            });
        }

        public async Task<ResponseDTO?> DeleteItemInCart(int cartId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.DELETE,
                Url = SD.EcommerceAPIBase + "/api/cart/" + cartId
            });
        }

		public async Task<ResponseDTO?> DeleteUserCart(string userId)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				APIType = SD.APIType.DELETE,
				Url = SD.EcommerceAPIBase + "/api/cart/delete-user-cart/" + userId
			});
		}
	}
}
