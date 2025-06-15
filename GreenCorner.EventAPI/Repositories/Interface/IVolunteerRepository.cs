using GreenCorner.EventAPI.Models;

namespace GreenCorner.EventAPI.Repositories.Interface
{
    public interface IVolunteerRepository
    {
        Task RegisteredVolunteer(Volunteer volunteer);
        Task<bool> IsVolunteer(int eventId, string userId);
        Task<bool> IsTeamLeader(int eventId, string userId);
        Task UnRegisteVolunteer(int eventId, string userId, string role);
        Task<bool> CheckRegister(int eventId, string userId, string role);
    }
}
