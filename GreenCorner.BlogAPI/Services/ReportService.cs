using AutoMapper;
using GreenCorner.BlogAPI.Models.DTOs;
using GreenCorner.BlogAPI.Models;
using GreenCorner.BlogAPI.Repositories.Interface;
using GreenCorner.BlogAPI.Services.Interface;
using GreenCorner.BlogAPI.Repositories;

namespace GreenCorner.BlogAPI.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;

        public ReportService(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        public async Task SubmitReport(ReportDTO reportDto)
        {
            Report report = _mapper.Map<Report>(reportDto);
            await _reportRepository.SubmitReport(report);
        }

        public async Task<IEnumerable<ReportDTO>> GetAllReports()
        {
            var reports = await _reportRepository.GetAllReports();
            return _mapper.Map<List<ReportDTO>>(reports);
        }
    }
}
