
using AutoMapper;
using GreenCorner.EventAPI.Models.DTO;
using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Repositories.Interface;
using GreenCorner.EventAPI.Services.Interface;
using GreenCorner.EventAPI.Repositories;

namespace GreenCorner.EventAPI.Services
{
    public class EventReviewService : IEventReviewService
    {
        private readonly IEventReviewRepository _eventReviewRepository;
        private readonly IMapper _mapper;

        public EventReviewService(IEventReviewRepository eventReviewRepository, IMapper mapper)
        {
            _eventReviewRepository = eventReviewRepository;
            _mapper = mapper;
        }

        public async Task RateEvent(EventReviewDTO eventDTO)
        {
            EventReview eventReview = _mapper.Map<EventReview>(eventDTO);
            await _eventReviewRepository.RateEvent(eventReview);
        }

        public async Task DeleteEventReview(int id)
        {
            await _eventReviewRepository.DeleteEventReview(id);
        }
        

        public async Task EditEventReview(EventReviewDTO eventReview)
        {
            EventReview event_review = _mapper.Map<EventReview>(eventReview);
            await _eventReviewRepository.EditEventReview(event_review);
        }
        public async Task<IEnumerable<EventReviewDTO>> ViewEventReviewHistory(String userID)
        {
            var events = await _eventReviewRepository.ViewEventReviewHistory(userID);
            return _mapper.Map<List<EventReviewDTO>>(events);
        }
        public async Task<EventReviewDTO> GetEventReviewById(int id)
        {
            var eventReview = await _eventReviewRepository.GetEventReviewById(id);
            return _mapper.Map<EventReviewDTO>(eventReview);
        }
        public async Task<IEnumerable<EventReviewDTO>> GetEventReviewsByEventIdAsync(int eventId)
        {
            var reviews = await _eventReviewRepository.GetEventReviewsByEventIdAsync(eventId);
            return _mapper.Map<IEnumerable<EventReviewDTO>>(reviews);
        }
    }
}
