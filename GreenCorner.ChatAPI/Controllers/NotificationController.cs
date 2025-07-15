using GreenCorner.ChatAPI.DTOs;
using GreenCorner.ChatAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.ChatAPI.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ResponseDTO _responseDTO;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
            _responseDTO = new ResponseDTO();
        }

        [HttpGet("user/{userId}")]
        public async Task<ResponseDTO> GetByUser(string userId)
        {
            try
            {
                var list = await _notificationService.GetNotificationsByUserIdAsync(userId);
                _responseDTO.Result = list;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPost("send")]
        public async Task<ResponseDTO> Send([FromBody] NotificationDTO dto)
        {
            try
            {
                await _notificationService.SendNotificationAsync(dto.UserId, dto.Title, dto.Message);
                _responseDTO.Message = "Gửi thành công";
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
