using GreenCorner.EventAPI.Data;
using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Models.DTO;
using GreenCorner.EventAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.EventAPI.Repositories
{
	public class LeaderRepository : ILeaderRepository
	{
		private readonly GreenCornerEventContext _context;
		public LeaderRepository(GreenCornerEventContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<EventVolunteer>> GetListVolunteer(int eventId)
		{
			return await _context.EventVolunteers
						 .Where(e => e.CleanEventId == eventId)
						 .ToListAsync();
		}

        public async Task AttendanceCheck(string userId, int eventId, bool check)
        {
            var volunteers = await _context.EventVolunteers
     .Where(e => e.CleanEventId == eventId)
     .ToListAsync();

            var volunteer = volunteers.FirstOrDefault(e => e.UserId == userId);
            if (check)
            {
                volunteer.AttendanceStatus = "Present";
            }
            else
            {
                volunteer.AttendanceStatus = "Absent";
            }
            if (volunteer == null)
            {
                throw new KeyNotFoundException($"Event Volunteer with ID {userId} not found.");
            }
            //product.IsDeleted = true;
            _context.EventVolunteers.Update(volunteer);
            await _context.SaveChangesAsync();
        }

		public async Task EditAttendance(string userId, int eventId)
		{
			var volunteers = await _context.EventVolunteers
	 .Where(e => e.CleanEventId == eventId)
	 .ToListAsync();

			var volunteer = volunteers.FirstOrDefault(e => e.UserId == userId);
			
				volunteer.AttendanceStatus = "Not Yet";
			
			if (volunteer == null)
			{
				throw new KeyNotFoundException($"Event Volunteer with ID {userId} not found.");
			}
			//product.IsDeleted = true;
			_context.EventVolunteers.Update(volunteer);
			await _context.SaveChangesAsync();
		}

		public async Task KickVolunteer(string userId, int eventId)
        {

            var eventVolunteer = await _context.EventVolunteers
        .FirstOrDefaultAsync(e => e.CleanEventId == eventId && e.UserId == userId);

            if (eventVolunteer == null)
            {
                throw new KeyNotFoundException($"Event Volunteer with ID {userId} not found.");
            }

            var volunteer = await _context.Volunteers
                .FirstOrDefaultAsync(v => v.UserId == userId);

            if (volunteer != null)
            {
                volunteer.Status = "Removed";
                _context.Volunteers.Update(volunteer);
                await _context.SaveChangesAsync();
            }

            _context.EventVolunteers.Remove(eventVolunteer);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CleanupEvent>> GetOpenEventsByTeamLeader(string userId)
        {
            return await (from ev in _context.CleanupEvents
                                join evv in _context.EventVolunteers
                                    on ev.CleanEventId equals evv.CleanEventId
                                where ev.Status == "Open"
                                      && evv.IsTeamLeader == true
                                      && evv.UserId == userId
                                select ev)
                                .ToListAsync();
        }
    }
}
