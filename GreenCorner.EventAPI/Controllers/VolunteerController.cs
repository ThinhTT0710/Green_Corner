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

        [HttpPut("updateregister")]
        public async Task<ResponseDTO> UpdateRegister([FromBody] VolunteerDTO volunteerDto)
        {
            try
            {
                await _volunteerService.UpdateRegister(volunteerDto);
                _responseDTO.IsSuccess = true;
                _responseDTO.Message = "Cập nhật đăng ký thành công.";
                _responseDTO.Result = true;
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
                _responseDTO.Result = false;
            }
            return _responseDTO;
        }

        [HttpGet("volunteer-registrations")]
        public async Task<ResponseDTO> GetAllVolunteerRegistrations()
        {
            try
            {
                var result = await _volunteerService.GetAllVolunteerRegistrations();
                _responseDTO.Result = result;
                _responseDTO.IsSuccess = true;
                _responseDTO.Message = "Lấy danh sách tình nguyện viên thành công.";
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        [HttpGet("volunteer-registration/{id}")]
        public async Task<ResponseDTO> GetVolunteerRegistrationById(int id)
        {
            try
            {
                var result = await _volunteerService.GetVolunteerRegistrationById(id);
                _responseDTO.Result = result;
                _responseDTO.IsSuccess = true;
                _responseDTO.Message = "Lấy chi tiết tình nguyện viên thành công.";
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        [HttpPost("approve-volunteer/{id}")]
        public async Task<ResponseDTO> ApproveVolunteer(int id)
        {
            try
            {
                await _volunteerService.ApproveVolunteerRegistration(id);
                _responseDTO.IsSuccess = true;
                _responseDTO.Message = "Phê duyệt tình nguyện viên thành công.";
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        [HttpGet("teamleader-registrations")]
        public async Task<ResponseDTO> GetAllTeamLeaderRegistrations()
        {
            try
            {
                var result = await _volunteerService.GetAllTeamLeaderRegistrations();
                _responseDTO.Result = result;
                _responseDTO.IsSuccess = true;
                _responseDTO.Message = "Lấy danh sách trưởng nhóm thành công.";
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        [HttpGet("teamleader-registration/{id}")]
        public async Task<ResponseDTO> GetTeamLeaderRegistrationById(int id)
        {
            try
            {
                var result = await _volunteerService.GetTeamLeaderRegistrationById(id);
                _responseDTO.Result = result;
                _responseDTO.IsSuccess = true;
                _responseDTO.Message = "Lấy chi tiết trưởng nhóm thành công.";
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        [HttpPost("approve-teamleader/{id}")]
        public async Task<ResponseDTO> ApproveTeamLeader(int id)
        {
            try
            {
                await _volunteerService.ApproveTeamLeaderRegistration(id);
                _responseDTO.IsSuccess = true;
                _responseDTO.Message = "Phê duyệt trưởng nhóm thành công.";
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        [HttpPost("reject-teamleader/{id}")]
        public async Task<ResponseDTO> RejectTeamLeader(int id)
        {
            try
            {
                await _volunteerService.RejectTeamLeaderRegistration(id);
                _responseDTO.IsSuccess = true;
                _responseDTO.Message = "Từ chối yêu cầu thành công.";
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        [HttpPost("reject-volunteer/{id}")]
        public async Task<ResponseDTO> RejectVolunteer(int id)
        {
            try
            {
                await _volunteerService.RejectVolunteerRegistration(id);
                _responseDTO.IsSuccess = true;
                _responseDTO.Message = "Từ chối yêu cầu thành công.";
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        [HttpGet("parti-activities")]
        public async Task<ResponseDTO> GetParticipatedActivities(string userId)
        {
            try
            {
                var result = await _volunteerService.GetParticipatedActivitiesByUserId(userId);
                _responseDTO.Result = result;
                _responseDTO.IsSuccess = true;
                _responseDTO.Message = "Lấy hoạt động đã tham gia thành công.";
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        [HttpGet("approved-role")]
        public async Task<ResponseDTO> GetApprovedRole([FromQuery] int eventId, [FromQuery] string userId)
        {
            try
            {
                var result = await _volunteerService.GetApprovedRoleAsync(eventId, userId);
                _responseDTO.Result = result;
                _responseDTO.IsSuccess = true;
                _responseDTO.Message = result != null
                    ? $"Vai trò đã được duyệt: {result}"
                    : "Người dùng chưa được phê duyệt trong sự kiện này.";
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        [HttpGet("has-teamleader")]
        public async Task<ResponseDTO> HasApprovedTeamLeader([FromQuery] int eventId)
        {
            try
            {
                var result = await _volunteerService.HasApprovedTeamLeaderAsync(eventId);
                _responseDTO.Result = result;
                _responseDTO.IsSuccess = true;
                _responseDTO.Message = result
                    ? "Đã có trưởng nhóm được phê duyệt cho sự kiện này."
                    : "Chưa có trưởng nhóm nào được phê duyệt.";
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

    }
}
