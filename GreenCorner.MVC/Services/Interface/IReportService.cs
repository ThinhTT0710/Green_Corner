using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IReportService
    {
        Task<ResponseDTO?> SubmitReport(ReportDTO report);
        Task<ResponseDTO?> GetAllReports();
    }
}
