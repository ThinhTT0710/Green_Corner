using GreenCorner.BlogAPI.Models;

namespace GreenCorner.BlogAPI.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllBlogPost();
        Task<IEnumerable<BlogPost>> GetAllPendingPost();
        Task<BlogPost> GetByBlogId(int id);
        Task AddBlog(BlogPost item);
        Task UpdateBlog(BlogPost item);
        Task DeleteBlog(int id);
        Task BlogApproval(int id);
    }
}
