using GreenCorner.EventAPI.Models.DTO;

namespace GreenCorner.EventAPI.Services.Interface
{
    public interface IEventReviewService
    {
        Task RateEvent(EventReviewDTO eventReview);

        Task<EventReviewDTO> GetEventReviewById(int id);
        Task EditEventReview(EventReviewDTO eventReview);
        Task DeleteEventReview(int id);
        Task<IEnumerable<EventReviewDTO>> ViewEventReviewHistory(String userID);
    }
}
