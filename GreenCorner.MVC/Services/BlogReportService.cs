using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;
using System.Reflection.Metadata;

namespace GreenCorner.MVC.Services
{
    public class BlogReportService : IBlogReportService
    {
        private readonly IBaseService _baseService;
        public BlogReportService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> CreateReportAsync(BlogReportDTO dto)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = dto,
                Url = SD.BlogAPIBase + "/api/BlogReport"
            });
        }

        public async Task<ResponseDTO?> EditReportAsync(int reportId, string newReason)
        {
            var dto = new
            {
                Reason = newReason
            };

            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.PUT,
                Data = dto,
                Url = $"{SD.BlogAPIBase}/api/BlogReport/{reportId}"
            });
        }

        public async Task<ResponseDTO?> GetReportsByBlogIdAsync(int blogId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.BlogAPIBase + "/api/BlogReport/by-blog/" + blogId
            });
        }
    }
}
