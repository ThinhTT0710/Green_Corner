using GreenCorner.EventAPI.Models;

namespace GreenCorner.EventAPI.Repositories.Interface
{
    public interface ILeaderReviewRepository
    {
        Task LeaderReview(LeaderReview item);
        Task EditLeaderReview(LeaderReview item);
        Task DeleteLeaderReview(int id);

		Task<LeaderReview> GetLeaderReviewById(int id);
		Task<IEnumerable<LeaderReview>> ViewLeaderReviewHistory(string reviewerID);
    }
}
