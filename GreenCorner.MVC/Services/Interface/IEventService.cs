using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IEventService
    {
        Task<ResponseDTO?> GetAllEvent();
        Task<ResponseDTO?> GetOpenEvent();
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
		    Task<ResponseDTO?> ViewEventVolunteerList(int id);
        Task<ResponseDTO?> AttendanceCheck(string userId, int eventId, bool check);
		    Task<ResponseDTO?> EditAttendance(string userId, int eventId);
        Task<ResponseDTO?> GetOpenEventsByTeamLeader(string userId);
        Task<ResponseDTO?> CreateCleanupEvent(EventDTO item);
        Task<ResponseDTO?> UpdateCleanupEvent(EventDTO item);
        Task<ResponseDTO?> UpdateCleanupEventStatus(EventDTO item);
        Task<ResponseDTO?> CloseCleanupEvent(int id);
		    Task<ResponseDTO?> OpenCleanupEvent(int id);
		    Task<ResponseDTO?> KickVolunteer(string userId, int eventId);
        Task<ResponseDTO?> GetEventsByIdsAsync(List<int> eventIds);
        Task<ResponseDTO?> GetParticipationInfoAsync(int eventId);
        Task<ResponseDTO?> CheckEventIsFullAsync(int eventId);
        Task<ResponseDTO?> DeleteVolunteersByEventId(int eventId);
        Task<ResponseDTO?> UpdateVolunteerStatusToParticipated(int eventId);
        Task<ResponseDTO?> GetTop3OpenEventsAsync();
    }
}
