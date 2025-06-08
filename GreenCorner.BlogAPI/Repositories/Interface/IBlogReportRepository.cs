using GreenCorner.BlogAPI.Models;

namespace GreenCorner.BlogAPI.Repositories.Interface
{
    public interface IBlogReportRepository
    {
        Task AddReport(BlogReport report);
        Task<IEnumerable<BlogReport>> GetReportsByBlogId(int blogId);
        Task<BlogReport?> GetReportById(int id);
        Task UpdateReport(BlogReport report);
    }
}
