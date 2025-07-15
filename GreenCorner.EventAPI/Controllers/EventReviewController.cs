using GreenCorner.EventAPI.Models.DTO;
using GreenCorner.EventAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.EventAPI.Controllers
{
    [Route("api/EventReview")]
    [ApiController]
    public class EventReviewController : ControllerBase
    {
        private readonly IEventReviewService _eventReviewService;   
        private readonly ResponseDTO _responseDTO;
        public EventReviewController(IEventReviewService eventReviewService)
        {
            _eventReviewService = eventReviewService;
            _responseDTO = new ResponseDTO();
        }
        [HttpPost]
        public async Task<ResponseDTO> RateEvent([FromBody] EventReviewDTO eventReview)
        {
            try
            {
                await _eventReviewService.RateEvent(eventReview);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Đánh giá sự kiện thành công!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
        [HttpGet("get-reviewid/{id}")]
        public async Task<ResponseDTO> GetEventReviewById(int id)
        {
            try
            {
                var eventReview = await _eventReviewService.GetEventReviewById(id);
                _responseDTO.Result = eventReview;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Lấy đánh giá sự kiện thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPut]
        public async Task<ResponseDTO> EditEventReview([FromBody] EventReviewDTO eventReview)
        {
            try
            {
                await _eventReviewService.EditEventReview(eventReview);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Cập nhật đánh giá sự kiện thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
        [HttpDelete("{id}")]
        public async Task<ResponseDTO> DeleteEventReivew(int id)
        {
            try
            {
                await _eventReviewService.DeleteEventReview(id);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Xóa đánh giá sự kiện thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
        [HttpGet("{id}")]
        public async Task<ResponseDTO> ViewEventReviewHistory(string id)
        {
            try
            {
                var eventReviewHistory = await _eventReviewService.ViewEventReviewHistory(id);
                _responseDTO.Result = eventReviewHistory;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Xem lịch sử đánh giá sự kiện thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
    }
}
