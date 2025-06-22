using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;

namespace GreenCorner.MVC.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IBaseService _baseService;
        public FeedbackService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> GetAllFeedback()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.BlogAPIBase + "/api/Feedback"
            });
        }

        public async Task<ResponseDTO?> SubmitFeedback(FeedbackDTO feedback)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = feedback,
                Url = SD.BlogAPIBase + "/api/Feedback"
            });
        }
    }
}
