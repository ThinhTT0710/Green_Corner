using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;

namespace GreenCorner.MVC.Services
{
    public class EventService : IEventService
    {
        private readonly IBaseService _baseService;
        public EventService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO?> GetAllEvent()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EventAPIBase + "/api/Event"
            });
        }
        public async Task<ResponseDTO?> GetByEventId(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EventAPIBase + "/api/Event/" + id
            });

        }
        public async Task<ResponseDTO?> GetEventReviewById(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EventAPIBase + "/api/EventReview/get-reviewid/" + id
            });
        }
		public async Task<ResponseDTO?> GetLeaderReviewById(int id)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				APIType = SD.APIType.GET,
				Url = SD.EventAPIBase + "/api/LeaderReview/get-reviewid/" + id
			});
		}

		public async Task<ResponseDTO?> DeleteEventReview(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.DELETE,
                Url = SD.EventAPIBase + "/api/EventReview/" + id
            });
        }

        public async Task<ResponseDTO?> DeleteLeaderReview(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.DELETE,
                Url = SD.EventAPIBase + "/api/LeaderReview/" + id
            });
        }

        public async Task<ResponseDTO?> EditEventReview(EventReviewDTO eventReviewDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.PUT,
                Data = eventReviewDTO,
                Url = SD.EventAPIBase + "/api/EventReview"
            });
        }

        public async Task<ResponseDTO?> EditLeaderReview(LeaderReviewDTO leaderReviewDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.PUT,
                Data = leaderReviewDTO,
                Url = SD.EventAPIBase + "/api/LeaderReview"
            });
        }

       

        public async Task<ResponseDTO?> LeaderReview(LeaderReviewDTO leaderReviewDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = leaderReviewDTO,
                Url = SD.EventAPIBase + "/api/LeaderReview"
            });
        }

        public async Task<ResponseDTO?> RateEvent(EventReviewDTO eventReviewDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = eventReviewDTO,
                Url = SD.EventAPIBase + "/api/EventReview"
            });
        }

        public async Task<ResponseDTO?> ViewEventReviewHistory(string id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EventAPIBase + "/api/EventReview/" + id
            }); 
        }

        public async Task<ResponseDTO?> ViewLeaderReviewHistory(string id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EventAPIBase + "/api/LeaderReview/" + id
            });
        }
    }
}
