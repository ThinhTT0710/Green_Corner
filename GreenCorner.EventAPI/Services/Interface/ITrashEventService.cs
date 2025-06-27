
using GreenCorner.EventAPI.Models.DTO;

namespace GreenCorner.EventAPI.Services.Interface
{
    public interface ITrashEventService
    {
        Task<IEnumerable<TrashEventDTO>> GetAllTrashEvent();
        Task<TrashEventDTO> GetByTrashEventId(int id);
        Task AddTrashEvent(TrashEventDTO TrashEventDTO);
        Task UpdateTrashEvent(TrashEventDTO TrashEventDTO);
        Task DeleteTrashEvent(int id);
        Task ApproveTrashEvent(int id);
		Task RejectTrashEvent(int id);

	}
}

