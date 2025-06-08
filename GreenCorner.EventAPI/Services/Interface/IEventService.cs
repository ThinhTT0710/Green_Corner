using GreenCorner.EventAPI.Models.DTO;

namespace GreenCorner.EventAPI.Services.Interface
{
    public interface IEventService
    {
        Task<IEnumerable<EventDTO>> GetAllEvent();
        Task<EventDTO> GetByEventId(int id);
        
    }
}
