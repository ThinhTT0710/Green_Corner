using GreenCorner.EventAPI.DTOs;
using GreenCorner.EventAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.EventAPI.Controllers
{
    [Route("api/trashevent")]
    [ApiController]
    public class TrashEventController : ControllerBase
    {
        private readonly ITrashEventService _trashEventService;
        private readonly ResponseDTO _responseDTO;

        public TrashEventController(ITrashEventService trashEventService)
        {
            _trashEventService = trashEventService;
            this._responseDTO = new ResponseDTO();
        }

        [HttpGet]
        public async Task<ResponseDTO> GetTrashEvents()
        {
            try
            {
                var trashEvents = await _trashEventService.GetAllTrashEvent();
                _responseDTO.Result = trashEvents;
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
        public async Task<ResponseDTO> GetTrashEventById(int id)
        {
            try
            {
                var trashEvents = await _trashEventService.GetByTrashEventId(id);
                _responseDTO.Result = trashEvents;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPost]
        public async Task<ResponseDTO> CreateTrashEvent([FromBody] TrashEventDTO trashEventDTO)
        {
            try
            {
                await _trashEventService.AddTrashEvent(trashEventDTO);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPut]
        public async Task<ResponseDTO> UpdateTrashEvent([FromBody] TrashEventDTO trashEventDTO)
        {
            try
            {
                await _trashEventService.UpdateTrashEvent(trashEventDTO);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
        [HttpDelete("{id}")]
        public async Task<ResponseDTO> DeleteTrashEvent(int id)
        {
            try
            {
                await _trashEventService.DeleteTrashEvent(id);
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
