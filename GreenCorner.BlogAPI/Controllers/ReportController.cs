using Azure;
using GreenCorner.BlogAPI.Models.DTO;
using GreenCorner.BlogAPI.Models.DTOs;
using GreenCorner.BlogAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ResponseDTO _responseDTO;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
            _responseDTO = new ResponseDTO();
        }
        [HttpPost]
        public async Task<ResponseDTO> SubmitReport([FromBody] ReportDTO report)
        {
            try
            {
                await _reportService.SubmitReport(report);
                _responseDTO.Message = "Báo cáo đã được gửi thành công.";
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = "Báo cáo gửi thất bại!";
            }

            return _responseDTO;
        }

        [HttpGet]
        public async Task<ResponseDTO> GetAllReports()
        {
            try
            {
                var data = await _reportService.GetAllReports();
                _responseDTO.Result = data;
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = "Không thể lấy danh sách báo cáo!";
            }

            return _responseDTO;
        }
    }
}
