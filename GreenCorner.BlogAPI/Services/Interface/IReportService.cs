using GreenCorner.BlogAPI.Models.DTOs;

namespace GreenCorner.BlogAPI.Services.Interface
{
    public interface IReportService
    {
        Task SubmitReport(ReportDTO report);
        Task<IEnumerable<ReportDTO>> GetAllReports();
    }
}
