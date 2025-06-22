using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IBlogFavoriteService
    {
        Task<ResponseDTO?> AddFavoriteAsync(BlogFavoriteAddDTO dto);
        Task<ResponseDTO?> RemoveFavoriteAsync(int blogId, string userId);
        Task<ResponseDTO?> IsFavoritedAsync(int blogId, string userId);
        Task<ResponseDTO?> GetFavoritesByUserAsync(string userId);
        Task<ResponseDTO?> ToggleFavoriteAsync(int blogId, string userId);

    }
}
