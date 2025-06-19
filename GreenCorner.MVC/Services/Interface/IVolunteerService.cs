using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IVolunteerService
    {
        Task<ResponseDTO?> RegisterVolunteer(VolunteerDTO dto);
        Task<ResponseDTO?> UnregisterAsync(int eventId, string userId, string role);
        Task<ResponseDTO?> IsVolunteer(int eventId, string userId);
        Task<ResponseDTO?> IsTeamLeader(int eventId, string userId);
        Task<ResponseDTO?> UpdateRegister(VolunteerDTO volunteer);
    }
}
