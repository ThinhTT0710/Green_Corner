
using GreenCorner.EventAPI.Models;

namespace GreenCorner.EventAPI.Repositories.Interface
{
    public interface IEventRepository
    {
        Task<IEnumerable<CleanupEvent>> GetAllEvent();
        Task<IEnumerable<CleanupEvent>> GetOpenEvent();
        Task<CleanupEvent> GetByEventId(int id);
        Task CreateCleanupEvent(CleanupEvent item);
        Task UpdateCleanupEvent(CleanupEvent item);
        Task UpdateCleanupEventStatus(CleanupEvent item);
        Task CloseCleanupEvent(int id);
		Task OpenCleanupEvent(int id);
	}
}
