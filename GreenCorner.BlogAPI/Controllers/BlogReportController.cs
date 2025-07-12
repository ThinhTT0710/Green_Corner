


using GreenCorner.BlogAPI.Models.DTO;
using GreenCorner.BlogAPI.Models.DTOs;
using GreenCorner.BlogAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogReportController : ControllerBase
    {
        private readonly IBlogReportService _blogReportService;
        private readonly ResponseDTO _responseDTO;
        public BlogReportController(IBlogReportService blogReportService)
        {
            _blogReportService = blogReportService;
            _responseDTO = new ResponseDTO();
        }
        [HttpPost]
        public async Task<ResponseDTO> CreateReport([FromBody] BlogReportDTO reportDto)
        {
            try
            {
                await _blogReportService.CreateReportAsync(reportDto);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("by-blog/{blogId}")]
        public async Task<ResponseDTO> GetReportsByBlogId(int blogId)
        {
            try
            {
                var reports = await _blogReportService.GetReportsByBlogIdAsync(blogId);
                _responseDTO.Result = reports;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPut("{reportId}")]
        public async Task<ResponseDTO> EditReport(int reportId, [FromBody] string newReason)
        {
            try
            {
                var updatedReport = await _blogReportService.EditReportAsync(reportId, newReason);

                if (updatedReport == null)
                {
                    _responseDTO.IsSuccess = false;
                    _responseDTO.Message = "Cannot edit report. It may not exist or edit time exceeded 2 hours.";
                    return _responseDTO;
                }

                _responseDTO.Result = updatedReport;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("by-reportid/{reportId}")]
        public async Task<ResponseDTO> GetReportById(int reportId)
        {
            try
            {
                var reports = await _blogReportService.GetReportById(reportId);
                _responseDTO.Result = reports;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
    }
}
