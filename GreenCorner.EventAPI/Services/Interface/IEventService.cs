using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Models.DTO;

namespace GreenCorner.EventAPI.Services.Interface
{
    public interface IEventService
    {
        Task<IEnumerable<EventDTO>> GetAllEvent();
        Task<EventDTO> GetByEventId(int id);
        Task CreateCleanupEvent(EventDTO item);
        Task UpdateCleanupEvent(EventDTO item);
        Task UpdateCleanupEventStatus(EventDTO item);
        Task CloseCleanupEvent(int id);

    }
}
