using GreenCorner.EventAPI.Models;

namespace GreenCorner.EventAPI.Repositories.Interface
{
	public interface ILeaderRepository
	{
		Task<IEnumerable<EventVolunteer>> GetListVolunteer(int eventId);

		Task AttendanceCheck (string userId, int eventId, bool check);

        Task KickVolunteer(string userId, int eventId);
    }
}
