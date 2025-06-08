using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;

namespace GreenCorner.MVC.Services
{
    public class BlogFavoriteService : IBlogFavoriteService
    {
        private readonly IBaseService _baseService;
        public BlogFavoriteService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> AddFavoriteAsync(BlogFavoriteAddDTO dto)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = dto,
                Url = SD.BlogAPIBase + "/api/BlogFavorite/add"
            });
        }

        public async Task<ResponseDTO?> GetFavoritesByUserAsync(string userId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.BlogAPIBase + $"/api/BlogFavorite/by-user?UserId={userId}"
            });
        }

        public async Task<ResponseDTO?> IsFavoritedAsync(int blogId, string userId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.BlogAPIBase + $"/api/BlogFavorite/is-favorited?BlogId={blogId}&UserId={userId}"
            });
        }

        public async Task<ResponseDTO?> RemoveFavoriteAsync(int blogId, string userId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.DELETE,
                Url = SD.BlogAPIBase + $"/api/BlogFavorite/remove?BlogId={blogId}&UserId={userId}"
            });
        }

        public async Task<ResponseDTO?> ToggleFavoriteAsync(int blogId, string userId)
        {
            // 1. Kiểm tra trạng thái đã yêu thích chưa
            var isFavoritedResponse = await IsFavoritedAsync(blogId, userId);
            if (isFavoritedResponse == null || !isFavoritedResponse.IsSuccess)
            {
                // Không thể kiểm tra trạng thái
                return new ResponseDTO
                {
                    IsSuccess = false,
                    Message = "Không thể kiểm tra trạng thái yêu thích."
                };
            }

            bool isFavorited = false;

            // Giả sử API trả về Result kiểu bool
            if (isFavoritedResponse.Result is bool boolResult)
            {
                isFavorited = boolResult;
            }
            else
            {
                // Nếu Result là string "true"/"false"
                bool.TryParse(isFavoritedResponse.Result?.ToString(), out isFavorited);
            }

            ResponseDTO? response;

            if (isFavorited)
            {
                // Nếu đã thích rồi => gọi API xóa
                response = await RemoveFavoriteAsync(blogId, userId);
            }
            else
            {
                // Nếu chưa thích => gọi API thêm
                var dto = new BlogFavoriteAddDTO { BlogId = blogId, UserId = userId };
                response = await AddFavoriteAsync(dto);
            }

            return response;
        }


    }
}
