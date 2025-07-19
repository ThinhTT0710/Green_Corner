using AutoMapper;
using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Models.DTO;
using GreenCorner.EventAPI.Repositories;
using GreenCorner.EventAPI.Repositories.Interface;
using GreenCorner.EventAPI.Services.Interface;
using Microsoft.Extensions.Logging;

namespace GreenCorner.EventAPI.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<EventDTO>> GetAllEvent()
        {
            var events =  await _eventRepository.GetAllEvent();
            return _mapper.Map<List<EventDTO>>(events);
        }
        public async Task<IEnumerable<EventDTO>> GetOpenEvent()
        {
            var events = await _eventRepository.GetOpenEvent();
            return _mapper.Map<List<EventDTO>>(events);
        }
        public async Task<EventDTO> GetByEventId(int id)
        {
            var cleanupEvent = await _eventRepository.GetByEventId(id);
            return _mapper.Map<EventDTO>(cleanupEvent);
        }

        public async Task CreateCleanupEvent(EventDTO eventDTO)
        {
            CleanupEvent cleanupEvent = _mapper.Map<CleanupEvent>(eventDTO);
            await _eventRepository.CreateCleanupEvent(cleanupEvent);
        }

        public async Task CloseCleanupEvent(int id)
        {
            await _eventRepository.CloseCleanupEvent(id);
        }
		public async Task OpenCleanupEvent(int id)
		{
			await _eventRepository.OpenCleanupEvent(id);
		}
		public async Task UpdateCleanupEvent(EventDTO eventDTO)
        {
            CleanupEvent cleanup = _mapper.Map<CleanupEvent>(eventDTO);
            await _eventRepository.UpdateCleanupEvent(cleanup);
        }
        public async Task UpdateCleanupEventStatus(EventDTO eventDTO)
        {
            CleanupEvent cleanup = _mapper.Map<CleanupEvent>(eventDTO);
            await _eventRepository.UpdateCleanupEventStatus(cleanup);
        }

        public async Task<List<EventDTO>> GetEventsByIdsAsync(List<int> eventIds)
        {
            var events = await _eventRepository.GetEventsByIdsAsync(eventIds);
            return _mapper.Map<List<EventDTO>>(events);
        }
        public async Task<(int currentCount, int max)> GetEventParticipationInfoAsync(int eventId)
        {
            var evt = await _eventRepository.GetByEventId(eventId);
            if (evt == null) throw new Exception("Event not found");

            int current = await _eventRepository.CountVolunteersByEventIdAsync(eventId);
            int max = evt.MaxParticipants ?? int.MaxValue;

            return (current, max);
        }

        public async Task<bool> IsEventFullAsync(int eventId)
        {
            var (current, max) = await GetEventParticipationInfoAsync(eventId);
            return current >= max;
        }

        public async Task<IEnumerable<EventDTO>> GetTop3OpenEventsAsync()
        {
            var events = await _eventRepository.GetTop3OpenEventsAsync();
            return _mapper.Map<List<EventDTO>>(events);
        }
    }
}
