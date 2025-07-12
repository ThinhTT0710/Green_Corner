    using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;

namespace GreenCorner.MVC.Services
{
    public class VolunteerService:IVolunteerService
    {
        private readonly IBaseService _baseService;
        public VolunteerService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> ApproveTeamLeaderRegistration(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Url = $"{SD.EventAPIBase}/api/Volunteer/approve-teamleader/{id}"
            });
        }

        public async Task<ResponseDTO?> ApproveVolunteerRegistration(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Url = $"{SD.EventAPIBase}/api/Volunteer/approve-volunteer/{id}"
            });
        }

        public async Task<ResponseDTO?> GetAllTeamLeaderRegistrations()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = $"{SD.EventAPIBase}/api/Volunteer/teamleader-registrations"
            });
        }

        public async Task<ResponseDTO?> GetAllVolunteerRegistrations()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = $"{SD.EventAPIBase}/api/Volunteer/volunteer-registrations"
            });
        }

        public async Task<ResponseDTO?> GetApprovedRoleAsync(int eventId, string userId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = $"{SD.EventAPIBase}/api/Volunteer/approved-role?eventId={eventId}&userId={userId}"
            });
        }

        public async Task<ResponseDTO?> GetParticipatedActivitiesByUserId(string userId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = $"{SD.EventAPIBase}/api/Volunteer/parti-activities?userId={userId}"
            });
        }

        public async Task<ResponseDTO?> GetTeamLeaderRegistrationById(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = $"{SD.EventAPIBase}/api/Volunteer/teamleader-registration/{id}"
            });
        }

        public async Task<ResponseDTO?> GetUserWithParticipation()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = $"{SD.EventAPIBase}/api/Volunteer/user-activities"
            });
        }

        public async Task<ResponseDTO?> GetVolunteerRegistrationById(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = $"{SD.EventAPIBase}/api/Volunteer/volunteer-registration/{id}"
            });
        }

        public async Task<ResponseDTO?> HasApprovedTeamLeaderAsync(int eventId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = $"{SD.EventAPIBase}/api/Volunteer/has-teamleader?eventId={eventId}"
            });
        }

        public async Task<ResponseDTO?> IsTeamLeader(int eventId, string userId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = $"{SD.EventAPIBase}/api/Volunteer/isteamleader?eventId={eventId}&userId={userId}"
            });
        }

        public async Task<ResponseDTO?> IsVolunteer(int eventId, string userId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = $"{SD.EventAPIBase}/api/Volunteer/isvolunteer?eventId={eventId}&userId={userId}"
            });
        }

        public async Task<ResponseDTO?> RegisterVolunteer(VolunteerDTO dto)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = dto,
                Url = SD.EventAPIBase + "/api/Volunteer/register"
            });
        }

        public async Task<ResponseDTO?> RejectTeamLeaderRegistration(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Url = $"{SD.EventAPIBase}/api/Volunteer/reject-teamleader/{id}"
            });
        }

        public async Task<ResponseDTO?> RejectVolunteerRegistration(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Url = $"{SD.EventAPIBase}/api/Volunteer/reject-volunteer/{id}"
            });
        }

        public async Task<ResponseDTO?> UnregisterAsync(int eventId, string userId, string role)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.DELETE,
                Url = $"{SD.EventAPIBase}/api/Volunteer/unregister?eventId={eventId}&userId={userId}&role={role}"
            });
        }

        public async Task<ResponseDTO?> UpdateRegister(VolunteerDTO volunteer)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.PUT,
                Data = volunteer,
                Url = SD.EventAPIBase + "/api/Volunteer/updateregister"
            });
        }
    }
}
