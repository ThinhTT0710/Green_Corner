
using GreenCorner.EventAPI.Models.DTO;
using GreenCorner.EventAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.EventAPI.Controllers
{
    [Route("api/Event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ResponseDTO _responseDTO;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
            _responseDTO = new ResponseDTO();
        }

        [HttpGet]
        public async Task<ResponseDTO> GetCleanupEvents()
        {
            try
            {
                var events = await _eventService.GetAllEvent();
                _responseDTO.Result = events;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
        [HttpGet("{id}")]
        public async Task<ResponseDTO> GetEventById(int id)
        {
            try
            {
                var cleanupEvent = await _eventService.GetByEventId(id);
                _responseDTO.Result = cleanupEvent;
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
