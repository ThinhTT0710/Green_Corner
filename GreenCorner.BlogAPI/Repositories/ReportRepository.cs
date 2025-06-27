using GreenCorner.BlogAPI.Data;
using GreenCorner.BlogAPI.Models;
using GreenCorner.BlogAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.BlogAPI.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly GreenCornerBlogContext _context;
        public ReportRepository(GreenCornerBlogContext context) { _context = context; }

        public async Task SubmitReport(Report report)
        {
            report.CreatedAt = DateTime.Now;
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Report>> GetAllReports()
        {
            return await _context.Reports.ToListAsync();
        }
    }
}
