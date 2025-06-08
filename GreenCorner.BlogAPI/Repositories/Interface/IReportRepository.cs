using GreenCorner.BlogAPI.Models;

namespace GreenCorner.BlogAPI.Repositories.Interface
{
    public interface IReportRepository
    {
        Task SubmitReport(Report report);
        Task<IEnumerable<Report>> GetAllReports();
    }
}
