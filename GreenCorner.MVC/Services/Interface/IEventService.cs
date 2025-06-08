using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IEventService
    {
        Task<ResponseDTO?> GetAllEvent();
        Task<ResponseDTO?> GetByEventId(int id);
        Task<ResponseDTO?> RateEvent(EventReviewDTO eventReviewDTO);
        Task<ResponseDTO?> EditEventReview(EventReviewDTO eventReviewDTO);
        Task<ResponseDTO?> GetEventReviewById(int id);
		Task<ResponseDTO?> GetLeaderReviewById(int id);
		Task<ResponseDTO?> DeleteEventReview(int id);
        Task<ResponseDTO?> ViewEventReviewHistory(string id);
        Task<ResponseDTO?> LeaderReview(LeaderReviewDTO leaderReviewDTO);
        Task<ResponseDTO?> EditLeaderReview(LeaderReviewDTO leaderReviewDTO);
        Task<ResponseDTO?> DeleteLeaderReview(int id);
        Task<ResponseDTO?> ViewLeaderReviewHistory(string id);
    }
}
