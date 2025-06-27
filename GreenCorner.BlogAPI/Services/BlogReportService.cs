using AutoMapper;
using GreenCorner.BlogAPI.Models;
using GreenCorner.BlogAPI.Models.DTOs;
using GreenCorner.BlogAPI.Repositories.Interface;
using GreenCorner.BlogAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.BlogAPI.Services
{
    public class BlogReportService : IBlogReportService
    {
        private readonly IBlogReportRepository _repository;
        private readonly IMapper _mapper;
        public BlogReportService(IBlogReportRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task CreateReportAsync(BlogReportDTO dto)
        {
            BlogReport blogReport = _mapper.Map<BlogReport>(dto);
            await _repository.AddReport(blogReport);
        }

        public async Task<BlogReportDTO?> EditReportAsync(int reportId, string newReason)
        {
            var report = await _repository.GetReportById(reportId);
            if (report == null)
                return null;


            if (report.CreatedAt == null || (DateTime.Now - report.CreatedAt.Value).TotalHours > 2)
                return null;

            report.Reason = newReason;
            await _repository.UpdateReport(report);

            return _mapper.Map<BlogReportDTO>(report);
        }


        public async Task<IEnumerable<BlogReportDTO>> GetReportsByBlogIdAsync(int blogId)
        {
            var reports = await _repository.GetReportsByBlogId(blogId);
            return reports.Select(r => _mapper.Map<BlogReportDTO>(r));
        }
    }
}
