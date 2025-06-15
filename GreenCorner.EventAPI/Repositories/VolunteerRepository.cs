using GreenCorner.EventAPI.Data;
using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.EventAPI.Repositories
{
    public class VolunteerRepository : IVolunteerRepository
    {
        private readonly GreenCornerEventContext _context;
        public VolunteerRepository(GreenCornerEventContext context)
        {
            _context = context;
        }

        public async Task<bool> IsVolunteer(int eventId, string userId)
        {
            return await _context.Volunteers
                .AnyAsync(v => v.CleanEventId == eventId && v.UserId == userId && v.ApplicationType == "Volunteer" && v.Status == "Pending");
        }

        public async Task<bool> IsTeamLeader(int eventId, string userId)
        {
            return await _context.Volunteers
                .AnyAsync(v => v.CleanEventId == eventId && v.UserId == userId && v.ApplicationType == "TeamLeader" && v.Status == "Pending");
        }


        public async Task RegisteredVolunteer(Volunteer volunteer)
        {
           volunteer.CreatedAt = DateTime.Now;
           volunteer.Status = "Pending";
           await _context.Volunteers.AddAsync(volunteer);
           await _context.SaveChangesAsync();
        }

        public async Task UnRegisteVolunteer(int eventId, string userId, string role)
        {
            var volunteer = await _context.Volunteers
                .FirstOrDefaultAsync(v => v.CleanEventId == eventId && v.UserId == userId && v.ApplicationType == role);

            if (volunteer != null)
            {
                _context.Volunteers.Remove(volunteer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CheckRegister(int eventId, string userId, string role)
        {
            return await _context.Volunteers
                .AnyAsync(v => v.CleanEventId == eventId && v.UserId == userId && v.ApplicationType == role && v.Status == "Pending");
        }
    }
}
