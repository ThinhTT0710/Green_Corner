using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Models.DTO;
using Microsoft.Extensions.Logging;

namespace GreenCorner.EventAPI.Services.Interface
{
    public interface IEventService
    {
        Task<IEnumerable<EventDTO>> GetAllEvent();
        Task<IEnumerable<EventDTO>> GetOpenEvent();
        Task<EventDTO> GetByEventId(int id);
        Task CreateCleanupEvent(EventDTO item);
        Task UpdateCleanupEvent(EventDTO item);
        Task UpdateCleanupEventStatus(EventDTO item);
        Task CloseCleanupEvent(int id);
		    Task OpenCleanupEvent(int id);
        Task<List<EventDTO>> GetEventsByIdsAsync(List<int> eventIds);
        Task<(int currentCount, int max)> GetEventParticipationInfoAsync(int eventId);
        Task<bool> IsEventFullAsync(int eventId);
        Task DeleteVolunteersByEventId(int eventId);
        Task UpdateVolunteerStatusToParticipated(int eventId);
        Task<IEnumerable<EventDTO>> GetTop3OpenEventsAsync();
    }
}
