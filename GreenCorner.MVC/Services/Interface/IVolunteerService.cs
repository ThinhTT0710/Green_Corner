using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IVolunteerService
    {
        Task<ResponseDTO?> RegisterVolunteer(VolunteerDTO dto);
        Task<ResponseDTO?> GetAllVolunteer();
        Task<ResponseDTO?> UnregisterAsync(int eventId, string userId, string role);
        Task<ResponseDTO?> IsVolunteer(int eventId, string userId);
        Task<ResponseDTO?> IsConfirmVolunteer(int eventId, string userId);
        Task<ResponseDTO?> IsTeamLeader(int eventId, string userId);
        Task<ResponseDTO?> UpdateRegister(VolunteerDTO volunteer);
        Task<ResponseDTO?>  GetAllVolunteerRegistrations();
        Task<ResponseDTO?>  GetVolunteerRegistrationById(int id);
        Task<ResponseDTO?> ApproveVolunteerRegistration(int id);
        Task<ResponseDTO?>  GetAllTeamLeaderRegistrations();
        Task<ResponseDTO?>  GetTeamLeaderRegistrationById(int id);
        Task<ResponseDTO?>  ApproveTeamLeaderRegistration(int id);
        Task<ResponseDTO?> RejectVolunteerRegistration(int id);
        Task<ResponseDTO?> RejectTeamLeaderRegistration(int id);
        Task<ResponseDTO?> GetParticipatedActivitiesByUserId(string userId);
        Task<ResponseDTO?> GetApprovedRoleAsync(int eventId, string userId);
        Task<ResponseDTO?> HasApprovedTeamLeaderAsync(int eventId);
        Task<ResponseDTO?> GetUserWithParticipation();
        Task<ResponseDTO?> GetApprovedVolunteersByUserIdAsync(string userId);

    }
}
