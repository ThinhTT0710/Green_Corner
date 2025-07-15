using GreenCorner.ChatAPI.DTOs;
using GreenCorner.ChatAPI.Services;
using GreenCorner.ChatAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.ChatAPI.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ResponseDTO _responseDTO;
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            this._responseDTO = new ResponseDTO();
            _chatService = chatService;
        }

        [HttpGet("messages/{eventId}")]
        public async Task<ResponseDTO> GetMessages(int eventId)
        {
            try
            {
                var messages = await _chatService.GetMessagesByEventIdAsync(eventId);
                _responseDTO.Result = messages;
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
