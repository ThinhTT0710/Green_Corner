using GreenCorner.BlogAPI.Models;
using GreenCorner.BlogAPI.Models.DTOs;

namespace GreenCorner.BlogAPI.Services.Interface
{
    public interface IBlogReportService
    {
        Task CreateReportAsync(BlogReportDTO dto);
        Task<IEnumerable<BlogReportDTO>> GetReportsByBlogIdAsync(int blogId);
        Task<BlogReportDTO?> EditReportAsync(int reportId, string newReason);
        Task<BlogReportDTO?> GetReportById(int id);
    }
}
