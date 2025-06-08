using GreenCorner.BlogAPI.Models;

namespace GreenCorner.BlogAPI.Repositories.Interface
{
    public interface IBlogFavoriteRepository
    {
        Task AddFavorite(BlogFavorite favorite);
        Task RemoveFavorite(int blogId, string userId);
        Task<bool> IsFavorited(int blogId, string userId);
        Task<IEnumerable<BlogFavorite>> GetFavoritesByUser(string userId);
    }
}
