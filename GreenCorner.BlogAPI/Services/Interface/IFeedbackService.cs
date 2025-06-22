using GreenCorner.BlogAPI.Models.DTOs;

namespace GreenCorner.BlogAPI.Services.Interface
{
    public interface IFeedbackService
    {
        Task SubmitFeedback(FeedbackDTO feedback);
        Task<IEnumerable<FeedbackDTO>> GetAllFeedback();
    }
}
