using GreenCorner.EventAPI.Models;

namespace GreenCorner.EventAPI.Repositories.Interface
{
	public interface ILeaderRepository
	{
		Task<IEnumerable<EventVolunteer>> GetListVolunteer(int eventId);

        Task<IEnumerable<CleanupEvent>> GetOpenEventsByTeamLeader(string userId);

        Task AttendanceCheck (string userId, int eventId, bool check);

		Task EditAttendance(string userId, int eventId);

		Task KickVolunteer(string userId, int eventId);
    }
}
