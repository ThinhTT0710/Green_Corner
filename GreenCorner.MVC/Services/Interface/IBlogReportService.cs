using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IBlogReportService
    {
        Task<ResponseDTO?> CreateReportAsync(BlogReportDTO dto);
        Task<ResponseDTO?> GetReportsByBlogIdAsync(int blogId);
        Task<ResponseDTO?> EditReportAsync(int reportId, string newReason);
    }
}
