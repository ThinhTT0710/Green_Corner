    using GreenCorner.EcommerceAPI.Models.DTO;
using GreenCorner.EcommerceAPI.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.EcommerceAPI.Controllers
{
    [Route("api/wishlist")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishListService;
        private readonly ResponseDTO _responseDTO;
        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
            this._responseDTO = new ResponseDTO();
        }
        [HttpGet]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<ResponseDTO> GetWishList()
        {
            try
            {
                var wishlist = await _wishListService.GetAll();
                _responseDTO.Result = wishlist;
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
        [Authorize(Roles = "CUSTOMER")]
        public async Task<ResponseDTO> GetWishListById(int id)
        {
            try
            {
                var wishList = await _wishListService.GetById(id);
                _responseDTO.Result = wishList;
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
        public async Task<ResponseDTO> AddToWishList([FromBody] WishListDTO wishListDTO)
        {
            try
            {
                await _wishListService.Add(wishListDTO);
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
        public async Task<ResponseDTO> UpdateWishList([FromBody] WishListDTO wishListDTO)
        {
            try
            {
                await _wishListService.Update(wishListDTO);
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
        public async Task<ResponseDTO> DeleteWishList(int id)
        {
            try
            {
                await _wishListService.Delete(id);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpDelete("delete-by-user/{userId}/{productId}")]
        public async Task<ResponseDTO> DeleteByUserId(string userId, int productId)
        {
            try
            {
                await _wishListService.DeleteByUserId(userId, productId);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }


        [HttpGet("get-user-wishlist/{userId}")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<ResponseDTO> GetUserWishList(string userId)
        {
            try
            {
                var wishLists = await _wishListService.GetByUserId(userId);
                _responseDTO.Result = wishLists;
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
