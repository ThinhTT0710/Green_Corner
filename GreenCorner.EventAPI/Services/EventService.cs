using AutoMapper;
using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Models.DTO;
using GreenCorner.EventAPI.Repositories.Interface;
using GreenCorner.EventAPI.Services.Interface;

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

        public async Task<EventDTO> GetByEventId(int id)
        {
            var cleanupEvent = await _eventRepository.GetByEventId(id);
            return _mapper.Map<EventDTO>(cleanupEvent);
        }

       
    }
}
