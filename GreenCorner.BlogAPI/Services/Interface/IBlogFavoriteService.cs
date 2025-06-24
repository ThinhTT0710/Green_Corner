using GreenCorner.BlogAPI.Models.DTOs;

namespace GreenCorner.BlogAPI.Services.Interface
{
    public interface IBlogFavoriteService
    {
        Task<bool> AddFavoriteAsync(BlogFavoriteAddDTO dto);
        Task<bool> RemoveFavoriteAsync(int blogId, string userId);
        Task<bool> IsFavoritedAsync(int blogId, string userId);
        Task<IEnumerable<BlogFavoriteDTO>> GetFavoritesByUserAsync(string userId);
    }
}
