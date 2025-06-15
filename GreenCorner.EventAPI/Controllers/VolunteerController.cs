using GreenCorner.EventAPI.Models.DTO;
using GreenCorner.EventAPI.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GreenCorner.EventAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerController : ControllerBase
    {
        private readonly IVolunteerService _volunteerService;
        private readonly ResponseDTO _responseDTO;
        public VolunteerController(IVolunteerService volunteerService)
        {
            _volunteerService = volunteerService;
            _responseDTO = new ResponseDTO();
        }

        [HttpPost("register")]
        public async Task<ResponseDTO> RegisterVolunteer([FromBody] VolunteerDTO volunteerDto)
        {
            try
            {
                string resultMessage = await _volunteerService.RegisterVolunteer(volunteerDto);
                _responseDTO.Message = resultMessage;
                _responseDTO.IsSuccess = true;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
                return _responseDTO;
            }
        }

        [HttpDelete("unregister")]
        public async Task<ResponseDTO> UnRegisterVolunteer([FromQuery] int eventId, [FromQuery] string userId, [FromQuery] string role)
        {
            try
            {
                string resultMessage = await _volunteerService.UnregisterAsync(eventId, userId, role);
                var failureMessages = new List<string>
                {
                    "Bạn chưa đăng ký làm tình nguyện viên nên không thể hủy.",
                    "Bạn chưa đăng ký làm đội trưởng nên không thể hủy.",
                    "Vai trò không hợp lệ."
                };
                _responseDTO.Message = resultMessage;
                _responseDTO.IsSuccess = !failureMessages.Contains(resultMessage);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
                return _responseDTO;
            }
        }

        [HttpGet("isvolunteer")]
        public async Task<ResponseDTO> IsVolunteer([FromQuery] int eventId, [FromQuery] string userId)
        {
            try
            {
                bool result = await _volunteerService.IsVolunteer(eventId, userId);
                _responseDTO.Result = result;
                if (result == true)
                {
                    _responseDTO.Message = "Volunteer registered, please wait for approval";
                    _responseDTO.IsSuccess = result;
                    

                }
                else
                {
                    _responseDTO.Message = "Not registered";
                    _responseDTO.IsSuccess = false;
                }

                    return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
                return _responseDTO;
            }
        }

        [HttpGet("isteamleader")]
        public async Task<ResponseDTO> IsTeamLeader([FromQuery] int eventId, [FromQuery] string userId)
        {
            try
            {
                bool result = await _volunteerService.IsTeamLeader(eventId, userId);
                _responseDTO.Result = result;
                if (result == true)
                {
                    _responseDTO.Message = "TeamLeader registered, please wait for approval";
                    _responseDTO.IsSuccess = result;
                    

                }
                else
                {
                    _responseDTO.Message = "Not registered";
                    _responseDTO.IsSuccess = false;
                }
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
                return _responseDTO;
            }
        }
    }
}
