using GreenCorner.EventAPI.Models;

namespace GreenCorner.EventAPI.Repositories.Interface
{
    public interface IEventReviewRepository
    {
        Task RateEvent(EventReview item);
        Task<EventReview> GetEventReviewById(int id);
        Task EditEventReview(EventReview item);
        Task DeleteEventReview(int id);
        Task<IEnumerable<EventReview>> ViewEventReviewHistory(string userId);

    }
}
