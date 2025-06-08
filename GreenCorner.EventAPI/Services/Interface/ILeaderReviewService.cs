using GreenCorner.EventAPI.Models.DTO;

namespace GreenCorner.EventAPI.Services.Interface
{
    public interface ILeaderReviewService
    {
        Task LeaderReview(LeaderReviewDTO leaderReviewDTO);
        Task EditLeaderReview(LeaderReviewDTO leaderReviewDTO);
        Task DeleteLeaderReview(int id);
		Task<LeaderReviewDTO> GetLeaderReviewById(int id);
		Task<IEnumerable<LeaderReviewDTO>> ViewLeaderReviewHistory(String reviewID);
    }
}
