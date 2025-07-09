using GreenCorner.EventAPI.Data;
using GreenCorner.EventAPI.Models;
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

        public async Task KickVolunteer(string userId, int eventId)
        {
            var volunteers = await _context.EventVolunteers
     .Where(e => e.CleanEventId == eventId)
     .ToListAsync();

            var volunteer = volunteers.FirstOrDefault(e => e.UserId == userId);
            if (volunteer == null)
            {
                throw new KeyNotFoundException($"Event Volunteer with ID {userId} not found.");
            }
            //product.IsDeleted = true;
            _context.EventVolunteers.Remove(volunteer);
            await _context.SaveChangesAsync();
        }
    }
}
