
using GreenCorner.EventAPI.Models.DTO;
using GreenCorner.EventAPI.Services;
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

        [HttpGet("get-openEvent")]
        public async Task<ResponseDTO> GetOpenCleanupEvents()
        {
            try
            {
                var events = await _eventService.GetOpenEvent();
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
                _responseDTO.Message = "Lấy thông tin sự kiện thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
        [HttpPost]
        public async Task<ResponseDTO> CreateCleanupEvent([FromBody] EventDTO eventDTO)
        {
            try
            {
                await _eventService.CreateCleanupEvent(eventDTO);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Tạo sự kiện vệ sinh thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
        [HttpPut]
        public async Task<ResponseDTO> UpdateCleanupEvent([FromBody] EventDTO eventDTO)
        {
            try
            {
                await _eventService.UpdateCleanupEvent(eventDTO);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Cập nhật thông tin sự kiện vệ sinh thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
        [HttpPut("status")]
        public async Task<ResponseDTO> UpdateCleanupEventStatus([FromBody] EventDTO eventDTO)
        {
            try
            {
                await _eventService.UpdateCleanupEventStatus(eventDTO);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Cập nhật trạng thái thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
        [HttpDelete("{id}")]
        public async Task<ResponseDTO> CloseCleanupEvent(int id)
        {
            try
            {
                await _eventService.CloseCleanupEvent(id);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Kết thúc sự kiện vệ sinh thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
		[HttpGet("open-event/{id}")]
		public async Task<ResponseDTO> OpenCleanupEvent(int id)
		{
			try
			{
				await _eventService.OpenCleanupEvent(id);
				return _responseDTO;
			}
			catch (Exception ex)
			{
				_responseDTO.Message = "Bắt đầu sự kiện thất bại!";
				_responseDTO.IsSuccess = false;
				return _responseDTO;
			}
		}

        [HttpPost("by-ids")]
        public async Task<ResponseDTO> GetEventsByIds([FromBody] List<int> ids)
        {
            var _responseDTO = new ResponseDTO();

            try
            {
                var result = await _eventService.GetEventsByIdsAsync(ids);

                _responseDTO.IsSuccess = true;
                _responseDTO.Message = "Lấy danh sách sự kiện thành công.";
                _responseDTO.Result = result;

                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = $"Đã xảy ra lỗi: {ex.Message}";
                _responseDTO.Result = null;

                return _responseDTO;
            }
        }
        [HttpGet("participation-info/{eventId}")]
        public async Task<ResponseDTO> GetParticipationInfo(int eventId)
        {
            var response = new ResponseDTO();

            try
            {
                var (current, max) = await _eventService.GetEventParticipationInfoAsync(eventId);
                response.IsSuccess = true;
                response.Result = new { Current = current, Max = max };
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpGet("is-full/{eventId}")]
        public async Task<ResponseDTO> IsEventFull(int eventId)
        {
            var response = new ResponseDTO();

            try
            {
                bool isFull = await _eventService.IsEventFullAsync(eventId);
                response.IsSuccess = true;
                response.Result = new { IsFull = isFull };
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpGet("top3event")]
        public async Task<ResponseDTO> Get3CleanupEvents()
        {
            try
            {
                var events = await _eventService.GetTop3OpenEventsAsync();
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
    }
}
