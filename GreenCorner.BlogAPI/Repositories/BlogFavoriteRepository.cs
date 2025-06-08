using GreenCorner.BlogAPI.Data;
using GreenCorner.BlogAPI.Models;
using GreenCorner.BlogAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.BlogAPI.Repositories
{
    public class BlogFavoriteRepository : IBlogFavoriteRepository
    {
        private  readonly GreenCornerBlogContext _context;
        public BlogFavoriteRepository(GreenCornerBlogContext context) 
        { 
            _context = context; 
        }

        public async Task AddFavorite(BlogFavorite favorite)
        {
            _context.BlogFavorites.Add(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFavorite(int blogId, string userId)
        {
            var favorite = await _context.BlogFavorites
                .FirstOrDefaultAsync(f => f.BlogId == blogId && f.UserId == userId);
            if (favorite != null)
            {
                _context.BlogFavorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsFavorited(int blogId, string userId)
        {
            return await _context.BlogFavorites
                .AnyAsync(f => f.BlogId == blogId && f.UserId == userId);
        }

        public async Task<IEnumerable<BlogFavorite>> GetFavoritesByUser(string userId)
        {
            return await _context.BlogFavorites
                .Include(f => f.Blog)
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }
    }
}
