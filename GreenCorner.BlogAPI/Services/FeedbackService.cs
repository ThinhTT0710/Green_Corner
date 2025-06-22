using AutoMapper;
using GreenCorner.BlogAPI.Models;
using GreenCorner.BlogAPI.Models.DTOs;
using GreenCorner.BlogAPI.Repositories;
using GreenCorner.BlogAPI.Repositories.Interface;
using GreenCorner.BlogAPI.Services.Interface;

namespace GreenCorner.BlogAPI.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IMapper _mapper;
        public FeedbackService(IFeedbackRepository feedbackRepository, IMapper mapper)
        {
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
        }
        public async Task SubmitFeedback(FeedbackDTO feedbackDto)
        {
            Feedback feedback = _mapper.Map<Feedback>(feedbackDto);
            await _feedbackRepository.SubmitFeedback(feedback);
        }
        public async Task<IEnumerable<FeedbackDTO>> GetAllFeedback()
        {
            var feedbacks = await _feedbackRepository.GetAllFeedback();
            return _mapper.Map<List<FeedbackDTO>>(feedbacks);
        }
    }
}
