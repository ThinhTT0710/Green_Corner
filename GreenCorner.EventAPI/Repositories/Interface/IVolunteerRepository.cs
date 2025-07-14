using GreenCorner.EventAPI.Models;

namespace GreenCorner.EventAPI.Repositories.Interface
{
    public interface IVolunteerRepository
    {
        Task RegisteredVolunteer(Volunteer volunteer);
        Task<bool> IsVolunteer(int eventId, string userId);
        Task<bool> IsConfirmVolunteer(int eventId, string userId);
        Task<bool> IsTeamLeader(int eventId, string userId);
        Task UnRegisteVolunteer(int eventId, string userId, string role);
        Task<bool> CheckRegister(int eventId, string userId, string role);
        Task UpdateRegister(Volunteer volunteer);
        Task<IEnumerable<Volunteer>> GetAllVolunteerRegistrations();
        Task<Volunteer> GetVolunteerRegistrationById(int id);
        Task ApproveVolunteerRegistration(int id);

        Task<IEnumerable<Volunteer>> GetAllTeamLeaderRegistrations();
        Task<Volunteer> GetTeamLeaderRegistrationById(int id);
        Task ApproveTeamLeaderRegistration(int id);
        Task RejectVolunteerRegistration(int id);
        Task RejectTeamLeaderRegistration(int id);

        Task<IEnumerable<Volunteer>> GetParticipatedActivitiesByUserId(string userId);

        Task<string> GetApprovedRoleAsync(int eventId, string userId);
        Task<bool> HasApprovedTeamLeaderAsync(int eventId);
        Task<IEnumerable<string>> GetUserWithParticipation();


    }
}
