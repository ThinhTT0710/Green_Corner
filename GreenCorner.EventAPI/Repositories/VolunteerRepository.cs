using GreenCorner.EventAPI.Data;
using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GreenCorner.EventAPI.Repositories
{
    public class VolunteerRepository : IVolunteerRepository
    {
        private readonly GreenCornerEventContext _context;
        public VolunteerRepository(GreenCornerEventContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Volunteer>> GetAllVolunteers()
        {
            return await _context.Volunteers.ToListAsync();
        }
        public async Task<Volunteer?> GetVolunteerByEventAndUserAsync(int eventId, string userId)
        {
            return await _context.Volunteers
                .FirstOrDefaultAsync(v => v.CleanEventId == eventId && v.UserId == userId);
        }

        public async Task<bool> IsVolunteer(int eventId, string userId)
        {
            return await _context.Volunteers
                .AnyAsync(v => v.CleanEventId == eventId && v.UserId == userId && v.ApplicationType == "Volunteer" && v.Status == "Pending");
        }
        public async Task<bool> IsConfirmVolunteer(int eventId, string userId)
        {
            return await _context.Volunteers
                .AnyAsync(v => v.CleanEventId == eventId && v.UserId == userId  && v.Status == "Approved");
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

        public async Task UpdateRegister(Volunteer volunteer)
        {
            var volunteers = await _context.Volunteers.FirstOrDefaultAsync(v => v.CleanEventId == volunteer.CleanEventId && v.UserId == volunteer.UserId);

            if (volunteers == null)
            {
                throw new KeyNotFoundException($"Volunteer with ID {volunteers.VolunteerId} not found.");
            }
            else if (volunteers.Status != "Pending")
            {
                throw new InvalidOperationException("Không thể chỉnh sửa đăng ký.");
            }
            //_context.Entry(volunteers).CurrentValues.SetValues(volunteer);
            volunteers.Assignment = volunteer.Assignment;
            volunteers.CarryItems = volunteer.CarryItems;
            volunteers.CreatedAt = DateTime.Now;
            volunteers.Status = "Pending";
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Volunteer>> GetAllVolunteerRegistrations()
        {
            return await _context.Volunteers.Where(v => v.ApplicationType != null && v.ApplicationType == "Volunteer" && v.Status == "Pending").ToListAsync();
        }

        public async Task<Volunteer> GetVolunteerRegistrationById(int id)
        {
            return await _context.Volunteers
                        .FirstOrDefaultAsync(v => v.VolunteerId == id && v.ApplicationType == "Volunteer")
                        ?? throw new KeyNotFoundException($"Không tìm thấy đơn đăng ký tình nguyện viên với ID = {id}");
        }

        public async Task ApproveVolunteerRegistration(int id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null)
            {
                throw new KeyNotFoundException($"Volunteer with ID {id} not found.");
            }
            volunteer.Status = "Approved";

            var @event = await _context.CleanupEvents.FindAsync(volunteer.CleanEventId);
            if (@event == null)
            {
                throw new KeyNotFoundException($"Event with ID {volunteer.CleanEventId} not found.");
            }

            var eventVolunteer = new EventVolunteer
            {
                CleanEventId = volunteer.CleanEventId,
                UserId = volunteer.UserId,
                IsTeamLeader = false,
                AttendanceStatus = "Not Yet", 
                PointsAwarded = @event.PointsAward,
                JoinDate = DateTime.Now,
                Note = string.IsNullOrWhiteSpace(volunteer.Assignment) && string.IsNullOrWhiteSpace(volunteer.CarryItems)
                    ? null
                    : string.Join(" - ", new[] { volunteer.Assignment, volunteer.CarryItems }.Where(s => !string.IsNullOrWhiteSpace(s)))
            };
            _context.EventVolunteers.Add(eventVolunteer);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Volunteer>> GetAllTeamLeaderRegistrations()
        {
            return await _context.Volunteers.Where(v => v.ApplicationType != null && v.ApplicationType == "TeamLeader" && v.Status == "Pending").ToListAsync();
        }

        public async Task<Volunteer> GetTeamLeaderRegistrationById(int id)
        {
            return await _context.Volunteers
                        .FirstOrDefaultAsync(v => v.VolunteerId == id && v.ApplicationType == "TeamLeader")
                        ?? throw new KeyNotFoundException($"Không tìm thấy đơn đăng ký trưởng nhóm với ID = {id}");
        }

        public async Task ApproveTeamLeaderRegistration(int id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null)
            {
                throw new KeyNotFoundException($"TeamLeader with ID {id} not found.");
            }
            volunteer.Status = "Approved";

            var eventVolunteer = new EventVolunteer
            {
                CleanEventId = volunteer.CleanEventId,
                UserId = volunteer.UserId,
                IsTeamLeader = true,
                AttendanceStatus = "Not Yet",
                PointsAwarded = 0,
                JoinDate = DateTime.Now,
                Note = string.IsNullOrWhiteSpace(volunteer.Assignment) && string.IsNullOrWhiteSpace(volunteer.CarryItems)
                    ? null
                    : string.Join(" - ", new[] { volunteer.Assignment, volunteer.CarryItems }.Where(s => !string.IsNullOrWhiteSpace(s)))
            };
            _context.EventVolunteers.Add(eventVolunteer);

            await _context.SaveChangesAsync();
        }

        public async Task RejectVolunteerRegistration(int id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null)
            {
                throw new KeyNotFoundException($"Volunteer with ID {id} not found.");
            }
            volunteer.Status = "Rejected";
            await _context.SaveChangesAsync();
        }

        public async Task RejectTeamLeaderRegistration(int id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null)
            {
                throw new KeyNotFoundException($"TeamLeader with ID {id} not found.");
            }
            volunteer.Status = "Rejected";
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Volunteer>> GetParticipatedActivitiesByUserId(string userId)
        {
            return await _context.Volunteers
                        .Include(v => v.CleanEvent)
                        .Where(v => v.UserId == userId)
                        .ToListAsync();
        }

        public async Task<string> GetApprovedRoleAsync(int eventId, string userId)
        {
            var record = await _context.Volunteers
            .FirstOrDefaultAsync(v =>
                v.CleanEventId == eventId &&
                v.UserId == userId &&
                v.Status == "Approved");

            return record == null ? null : record.ApplicationType;
        }

        public async Task<bool> HasApprovedTeamLeaderAsync(int eventId)
        {
            return await _context.Volunteers
            .AnyAsync(v =>
                v.CleanEventId == eventId &&
                v.ApplicationType == "TeamLeader" &&
                v.Status == "Approved");
        }

        public async Task<IEnumerable<string>> GetUserWithParticipation()
        {
            return await _context.Volunteers
                                .Select(v => v.UserId)
                                .Distinct()
                                .ToListAsync();
        }

        public async Task<List<Volunteer>> GetApprovedVolunteersByUserIdAsync(string userId)
        {
            return await _context.Volunteers
                        .Where(v => v.UserId == userId && v.Status == "Approved")
                        .ToListAsync();
        }
        public async Task<string> GetTeamLeaderByEventId(int eventId)
        {
            var record = await _context.Volunteers
            .FirstOrDefaultAsync(v =>
                v.CleanEventId == eventId &&
                v.ApplicationType == "TeamLeader" && v.Status == "Approved");

            return record == null ? null : record.UserId;
        }
    }
}
