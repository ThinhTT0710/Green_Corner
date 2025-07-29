using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IBlogPostService
    {
        Task<ResponseDTO?> GetAllBlogPost();
        Task<ResponseDTO?> GetAllPendingPost();
        Task<ResponseDTO?> GetByBlogId(int id);
        Task<ResponseDTO?> AddBlog(BlogPostDTO blog);
        Task<ResponseDTO?> UpdateBlog(BlogPostDTO blog);
        Task<ResponseDTO?> DeleteBlog(int id);
        Task<ResponseDTO?> BlogApproval(int id);
        Task<ResponseDTO?> GetBlogCreate(string userId);
        Task<ResponseDTO?> BlogReject(int id);
    }
}
