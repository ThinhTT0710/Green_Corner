using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Models.DTO;

namespace GreenCorner.EventAPI.Services.Interface
{
	public interface ILeaderService
	{
		Task<IEnumerable<EventVolunteerDTO>> ViewVolunteerList( int eventId);
        Task<IEnumerable<EventDTO>> GetOpenEventsByTeamLeader(string userId);
        Task AttendanceCheck(string userId, int eventId, bool check);

		Task EditAttendance(string userId, int eventId);
		Task KickVolunteer(string userId, int eventId);
    }
}
