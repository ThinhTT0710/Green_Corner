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
        Task<bool> IsConfirmVolunteer(int eventId, string userId);
        Task<bool> IsTeamLeader(int eventId, string userId);
        Task UpdateRegister(VolunteerDTO volunteer);
        Task<IEnumerable<VolunteerDTO>> GetAllVolunteerRegistrations();
        Task<VolunteerDTO> GetVolunteerRegistrationById(int id);
        Task ApproveVolunteerRegistration(int id);

        Task<IEnumerable<VolunteerDTO>> GetAllTeamLeaderRegistrations();
        Task<VolunteerDTO> GetTeamLeaderRegistrationById(int id);
        Task ApproveTeamLeaderRegistration(int id);
        Task RejectVolunteerRegistration(int id);
        Task RejectTeamLeaderRegistration(int id);

        Task<IEnumerable<VolunteerDTO>> GetParticipatedActivitiesByUserId(string userId);

        Task<string> GetApprovedRoleAsync(int eventId, string userId);
        Task<bool> HasApprovedTeamLeaderAsync(int eventId);

        Task<IEnumerable<string>> GetUserWithParticipation();
    }
}
