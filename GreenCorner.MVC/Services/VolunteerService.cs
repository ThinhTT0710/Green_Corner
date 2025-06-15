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

        public async Task<ResponseDTO?> UnregisterAsync(int eventId, string userId, string role)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.DELETE,
                Url = $"{SD.EventAPIBase}/api/Volunteer/unregister?eventId={eventId}&userId={userId}&role={role}"
            });
        }
    }
}
