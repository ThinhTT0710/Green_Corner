
using GreenCorner.EventAPI.Models;

namespace GreenCorner.EventAPI.Repositories.Interface
{
    public interface IEventRepository
    {
        Task<IEnumerable<CleanupEvent>> GetAllEvent();
        Task<CleanupEvent> GetByEventId(int id);
    }
}
