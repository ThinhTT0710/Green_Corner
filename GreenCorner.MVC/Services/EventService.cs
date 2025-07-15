using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;
using System.Net.Http;

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
        public async Task<ResponseDTO?> GetOpenEvent()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EventAPIBase + "/api/Event/get-openEvent"
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
		public async Task<ResponseDTO?> ViewEventVolunteerList(int id)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				APIType = SD.APIType.GET,
				Url = SD.EventAPIBase + "/api/Leader/" + id
			});
		}
        public async Task<ResponseDTO?> AttendanceCheck(string userId, int eventId, bool check)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EventAPIBase + "/api/Leader/" + userId+","+eventId+","+check
            });
        }
		public async Task<ResponseDTO?> EditAttendance(string userId, int eventId)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				APIType = SD.APIType.GET,
				Url = SD.EventAPIBase + "/api/Leader/edit-attendance/" + userId + "," + eventId 
			});
		}
        public async Task<ResponseDTO?> GetOpenEventsByTeamLeader(string userId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EventAPIBase + "/api/Leader/getEvent-byLeader/" + userId
            });
        }
        public async Task<ResponseDTO?> CreateCleanupEvent(EventDTO eventDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = eventDTO,
                Url = SD.EventAPIBase + "/api/Event"
            });
        }
        public async Task<ResponseDTO?> UpdateCleanupEvent(EventDTO eventDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.PUT,
                Data = eventDTO,
                Url = SD.EventAPIBase + "/api/Event"
            });
        }
        public async Task<ResponseDTO?> UpdateCleanupEventStatus(EventDTO eventDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.PUT,
                Data = eventDTO,
                Url = SD.EventAPIBase + "/api/Event/status"
            });
        }
        public async Task<ResponseDTO?> CloseCleanupEvent(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.DELETE,
                Url = SD.EventAPIBase + "/api/Event/" + id
            });
        }
		public async Task<ResponseDTO?> OpenCleanupEvent(int id)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				APIType = SD.APIType.GET,
				Url = SD.EventAPIBase + "/api/Event/open-event" + id
			});
		}
		public async Task<ResponseDTO?> KickVolunteer(string userId, int eventId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.DELETE,
                Url = SD.EventAPIBase + "/api/Leader/" + userId + "," + eventId 
            });
        }

        public async Task<ResponseDTO?> GetEventsByIdsAsync(List<int> ids)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = ids,
                Url = $"{SD.EventAPIBase}/api/Event/by-ids"
            });
        }

        public async Task<ResponseDTO?> GetParticipationInfoAsync(int eventId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = $"{SD.EventAPIBase}/api/Event/participation-info/{eventId}"
            });
        }



        public async Task<ResponseDTO?> CheckEventIsFullAsync(int eventId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = $"{SD.EventAPIBase}/api/Event/is-full/{eventId}"
            });
        }
    }
}
