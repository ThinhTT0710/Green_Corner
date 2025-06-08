using AutoMapper;
using GreenCorner.EventAPI.Models.DTO;
using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Repositories.Interface;
using GreenCorner.EventAPI.Services.Interface;
using GreenCorner.EventAPI.Repositories;

namespace GreenCorner.EventAPI.Services
{
    public class LeaderReviewService : ILeaderReviewService
    {
        private readonly ILeaderReviewRepository _leaderReviewRepository;
        private readonly IMapper _mapper;

        public LeaderReviewService(ILeaderReviewRepository leaderReviewRepository, IMapper mapper)

        {
            _leaderReviewRepository = leaderReviewRepository;
            _mapper = mapper;
        }

        public async Task LeaderReview(LeaderReviewDTO leaderReviewDTO)
        {
            LeaderReview leaderReview = _mapper.Map<LeaderReview>(leaderReviewDTO);
            await _leaderReviewRepository.LeaderReview(leaderReview);
        }

        public async Task DeleteLeaderReview(int id)
        {
            await _leaderReviewRepository.DeleteLeaderReview(id);
        }
		public async Task<LeaderReviewDTO> GetLeaderReviewById(int id)
		{
			var leaderReview = await _leaderReviewRepository.GetLeaderReviewById(id);
			return _mapper.Map<LeaderReviewDTO>(leaderReview);
		}

		public async Task EditLeaderReview(LeaderReviewDTO leaderReviewDTO)
        {
            LeaderReview leaderReview = _mapper.Map<LeaderReview>(leaderReviewDTO);
            await _leaderReviewRepository.EditLeaderReview(leaderReview);
        }
        public async Task<IEnumerable<LeaderReviewDTO>> ViewLeaderReviewHistory(String reviewerID)
        {
            var leaderReviews = await _leaderReviewRepository.ViewLeaderReviewHistory(reviewerID);
            return _mapper.Map<List<LeaderReviewDTO>>(leaderReviews);
        }
    }
}
