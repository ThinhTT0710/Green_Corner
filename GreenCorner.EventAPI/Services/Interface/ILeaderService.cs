using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Models.DTO;

namespace GreenCorner.EventAPI.Services.Interface
{
	public interface ILeaderService
	{
		Task<IEnumerable<EventVolunteerDTO>> ViewVolunteerList(int eventId);

		Task AttendanceCheck(string userId, int eventId, bool check);
	}
}
