using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Models.DTO;

namespace GreenCorner.EventAPI.Services.Interface
{
    public interface IVolunteerService
    {
        Task<string> RegisterVolunteer(VolunteerDTO dto);
        Task<string> UnregisterAsync(int eventId, string userId, string role);
        Task<bool> CheckRegisteredAsync(int eventId, string userId, string applicationType);
        Task<bool> IsVolunteer(int eventId, string userId);
        Task<bool> IsTeamLeader(int eventId, string userId);
        Task UpdateRegister(VolunteerDTO volunteer);
    }
}
