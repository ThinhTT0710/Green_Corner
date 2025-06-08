using GreenCorner.BlogAPI.Models;

namespace GreenCorner.BlogAPI.Repositories.Interface
{
    public interface IFeedbackRepository
    {
        Task SubmitFeedback(Feedback feedback);
        Task<IEnumerable<Feedback>> GetAllFeedback();
    }
}
