using GreenCorner.BlogAPI.Models;
using GreenCorner.BlogAPI.Models.DTOs;

namespace GreenCorner.BlogAPI.Services.Interface
{
    public interface IBlogPostService
    {
        Task<IEnumerable<BlogPostDTO>> GetAllBlogPost();
        Task<IEnumerable<BlogPostDTO>> GetAllPendingPost();
        Task<BlogPostDTO> GetByBlogId(int id);
        Task AddBlog(BlogPostDTO blog);
        Task UpdateBlog(BlogPostDTO blog);
        Task DeleteBlog(int id);
        Task BlogApproval (int id);
        Task<IEnumerable<BlogPostDTO>> GetBlogCreate(string userId);
    }
}
