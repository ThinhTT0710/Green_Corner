using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface ITrashEventService
    {
        Task<ResponseDTO?> GetAllTrashEvent();
        Task<ResponseDTO?> GetByTrashEventId(int id);
        Task<ResponseDTO?> AddTrashEvent(TrashEventDTO trashEventDTO);
        Task<ResponseDTO?> UpdateTrashEvent(TrashEventDTO trashEventDTO);
        Task<ResponseDTO?> DeleteTrashEvent(int id);
        Task<ResponseDTO?> ApproveTrashEvent(int id);
		Task<ResponseDTO?> RejectTrashEvent(int id);
        Task<ResponseDTO?> GetTrashEventsByUserId(string userId);
    }
}
