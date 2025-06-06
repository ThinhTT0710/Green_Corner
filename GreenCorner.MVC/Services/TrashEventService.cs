using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;

namespace GreenCorner.MVC.Services
{
    public class TrashEventService : ITrashEventService
    {
        private readonly IBaseService _baseService;

        public TrashEventService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> AddTrashEvent(TrashEventDTO trashEventDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = trashEventDTO,
                Url = SD.EventAPIBase + "/api/trashevent"
            });
        }

        public async Task<ResponseDTO?> DeleteTrashEvent(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.DELETE,
                Url = SD.EventAPIBase + "/api/trashevent/" + id
            });
        }

        public async Task<ResponseDTO?> GetAllTrashEvent()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EventAPIBase + "/api/trashevent"
            });
        }

        public async Task<ResponseDTO?> GetByTrashEventId(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EventAPIBase + "/api/trashevent/" + id
            });
        }

        public async Task<ResponseDTO?> UpdateTrashEvent(TrashEventDTO trashEventDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.PUT,
                Data = trashEventDTO,
                Url = SD.EventAPIBase + "/api/trashevent"
            });
        }
    }
}
