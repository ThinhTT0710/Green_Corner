using GreenCorner.BlogAPI.Data;
using GreenCorner.BlogAPI.Models;
using GreenCorner.BlogAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.BlogAPI.Repositories
{
    public class BlogReportRepository : IBlogReportRepository
    {
        private readonly GreenCornerBlogContext _context;
        public BlogReportRepository(GreenCornerBlogContext context)
        {
            _context = context;
        }

        public async Task AddReport(BlogReport report)
        {
            report.CreatedAt = DateTime.Now;
            await _context.BlogReports.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        public async Task<BlogReport?> GetReportById(int id)
        {
            return await _context.BlogReports
               .FirstOrDefaultAsync(p => p.BlogReportId == id)
               ?? throw new KeyNotFoundException($"Blog Report with ID {id} not found.");
        }

        public async Task<IEnumerable<BlogReport>> GetReportsByBlogId(int blogId)
        {
            return await _context.BlogReports
            .Where(r => r.BlogId == blogId)
            .Include(r => r.Blog)
            .ToListAsync();
        }

        public async Task UpdateReport(BlogReport report)
        {
            _context.BlogReports.Update(report);
            await _context.SaveChangesAsync();
        }
    }
}
