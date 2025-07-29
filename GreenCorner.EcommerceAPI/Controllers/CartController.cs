    using GreenCorner.EcommerceAPI.Models.DTO;
using GreenCorner.EcommerceAPI.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.EcommerceAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ResponseDTO _responseDTO;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
            _responseDTO = new ResponseDTO();
        }

        [HttpGet]
        public async Task<ResponseDTO> GetCarts()
        {
            try
            {
                var carts = await _cartService.GetAll();
                _responseDTO.Result = carts;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("{id}")]
        public async Task<ResponseDTO> GetCartById(int id)
        {
            try
            {
                var cart = await _cartService.GetById(id);
                _responseDTO.Result = cart;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPost]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<ResponseDTO> AddToCart([FromBody] CartDTO cartDTO)
        {
            try
            {
                if (cartDTO.Quantity <= 0)
                {
                    _responseDTO.Message = "Quantity must be greater than 0";
                    _responseDTO.IsSuccess = false;
                    return _responseDTO;
                }
                await _cartService.AddToCart(cartDTO);
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
            }
            return _responseDTO;
        }

        [HttpPut]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<ResponseDTO> UpdateCart([FromBody] CartDTO cartDTO)
        {
            try
            {
                if (cartDTO.Quantity <= 0)
                {
                    _responseDTO.Message = "Quantity must be greater than 0";
                    _responseDTO.IsSuccess = false;
                    return _responseDTO;
                }

                await _cartService.UpdateCart(cartDTO);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<ResponseDTO> DeleteCart(int id)
        {
            try
            {
                await _cartService.DeleteCart(id);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

		[HttpDelete("delete-user-cart/{userId}")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<ResponseDTO> DeleteUserCart(string userId)
		{
			try
			{
				await _cartService.DeleteUserCart(userId);
				return _responseDTO;
			}
			catch (Exception ex)
			{
				_responseDTO.Message = ex.Message;
				_responseDTO.IsSuccess = false;
				return _responseDTO;
			}
		}

		[HttpGet("get-user-cart/{userId}")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<ResponseDTO> GetUserCart(string userId)
        {
            try
            {
                var cart = await _cartService.GetByUserId(userId);
                _responseDTO.Result = cart;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
    }
}
