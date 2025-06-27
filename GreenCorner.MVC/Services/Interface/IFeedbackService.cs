using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IFeedbackService
    {
        Task<ResponseDTO?> SubmitFeedback(FeedbackDTO feedback);
        Task<ResponseDTO?> GetAllFeedback();
    }
}
