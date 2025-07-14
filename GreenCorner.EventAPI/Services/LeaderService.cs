using AutoMapper;
using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Models.DTO;
using GreenCorner.EventAPI.Repositories;
using GreenCorner.EventAPI.Repositories.Interface;
using GreenCorner.EventAPI.Services.Interface;
using Microsoft.Extensions.Logging;

namespace GreenCorner.EventAPI.Services
{
	public class LeaderService : ILeaderService
	{
		private readonly ILeaderRepository _leaderRepository;
		private readonly IMapper _mapper;
		public LeaderService(ILeaderRepository leaderRepository, IMapper mapper)

		{
			_leaderRepository = leaderRepository;
			_mapper = mapper;
		}
		public async Task<IEnumerable<EventVolunteerDTO>> ViewVolunteerList( int eventId)
		{
			var eventVolunteer = await _leaderRepository.GetListVolunteer(eventId);
			return _mapper.Map<List<EventVolunteerDTO>>(eventVolunteer);
		}

       

        public async Task AttendanceCheck(string userId, int eventId, bool check)
        {
            await _leaderRepository.AttendanceCheck(userId, eventId, check);
        }
		public async Task EditAttendance(string userId, int eventId)
		{
			await _leaderRepository.EditAttendance(userId, eventId);
		}

		public async Task KickVolunteer(string userId, int eventId)
        {
            await _leaderRepository.KickVolunteer(userId, eventId);
        }

        public async Task<IEnumerable<EventDTO>> GetOpenEventsByTeamLeader(string userId)
        {
            var events = await _leaderRepository.GetOpenEventsByTeamLeader(userId);
            return _mapper.Map<List<EventDTO>>(events);
        }
    }
}
