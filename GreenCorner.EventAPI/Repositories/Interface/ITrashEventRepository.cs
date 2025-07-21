using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Models.DTO;

namespace GreenCorner.EventAPI.Repositories.Interface
{
    public interface ITrashEventRepository
    {
        Task<IEnumerable<TrashEvent>> GetAllTrashEvent();
        Task<TrashEvent> GetByTrashEventId(int id);
        Task AddTrashEvent(TrashEvent TrashEvent);
        Task UpdateTrashEvent(TrashEvent TrashEvent);
        Task DeleteTrashEvent(int id);
        Task ApproveTrashEvent(int id);
		Task RejectTrashEvent(int id);
        Task<IEnumerable<TrashEvent>> GetByUserId(string userId);
        Task<MonthlyEventAnalyticsDTO> GetMonthlyEventAnalytics(int year);
    }
}
