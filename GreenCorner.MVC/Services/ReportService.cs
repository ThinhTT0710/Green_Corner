using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;
using System.ComponentModel;

namespace GreenCorner.MVC.Services
{
    public class ReportService : IReportService
    {
        private readonly IBaseService _baseService;
        public ReportService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> GetAllReports()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.BlogAPIBase + "/api/Report"
            });
        }

        public async Task<ResponseDTO?> SubmitReport(ReportDTO report)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = report,
                Url = SD.BlogAPIBase + "/api/Report"
            });
        }
    }
}
