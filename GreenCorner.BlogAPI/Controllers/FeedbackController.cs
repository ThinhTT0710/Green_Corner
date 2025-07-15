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
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        private readonly ResponseDTO _responseDTO;
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
            this._responseDTO = new ResponseDTO();
        }
        [HttpPost]
        public async Task<ResponseDTO> SubmitFeedback([FromBody] FeedbackDTO feedback)
        {
            try
            {
                await _feedbackService.SubmitFeedback(feedback);
                _responseDTO.Message = "Phản hồi đã được gửi thành công.";
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = "Phản hồi gửi thất bại!";
            }

            return _responseDTO;
        }

        [HttpGet]
        public async Task<ResponseDTO> GetAllFeedback()
        {
            try
            {
                var data = await _feedbackService.GetAllFeedback();
                _responseDTO.Result = data;
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = "Lấy danh sách phản hồi thất bại!";
            }

            return _responseDTO;
        }
    }
}
