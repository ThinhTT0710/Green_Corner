using GreenCorner.BlogAPI.Models.DTO;
using GreenCorner.BlogAPI.Models.DTOs;
using GreenCorner.BlogAPI.Services;
using GreenCorner.BlogAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogFavoriteController : ControllerBase
    {
        private readonly IBlogFavoriteService _blogFavoriteService;
        private readonly ResponseDTO _responseDTO;
        public BlogFavoriteController(IBlogFavoriteService blogFavoriteService)
        {
            _blogFavoriteService = blogFavoriteService;
            _responseDTO = new ResponseDTO();
        }
        [HttpPost("add")]
        public async Task<ResponseDTO> AddFavorite([FromBody] BlogFavoriteAddDTO dto)
        {
            try 
            {
                await _blogFavoriteService.AddFavoriteAsync(dto);
                return _responseDTO;
            }
            catch (Exception ex) 
            {
                _responseDTO.Message = "Yêu thích Blog thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpDelete("remove")]
        public async Task<ResponseDTO> RemoveFavorite(int blogId, string userId)
        {
            try
            {
                await _blogFavoriteService.RemoveFavoriteAsync(blogId, userId);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Xóa Blog ra khỏi danh sách yêu thích thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("is-favorited")]
        public async Task<ResponseDTO> IsFavorited(int blogId, string userId)
        {
            try
            {
                var isFav = await _blogFavoriteService.IsFavoritedAsync(blogId, userId);
                _responseDTO.Result = isFav;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
            
        }

        [HttpGet("by-user")]
        public async Task<ResponseDTO> GetFavoritesByUser(string userId)
        {
            try
            {
                var result = await _blogFavoriteService.GetFavoritesByUserAsync(userId);
                _responseDTO.Result=result;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Lấy danh sách Blog yêu thích thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
    }
}
