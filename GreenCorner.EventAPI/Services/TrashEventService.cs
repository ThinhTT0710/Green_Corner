using AutoMapper;
using GreenCorner.EventAPI.Models;
using GreenCorner.EventAPI.Models.DTO;
using GreenCorner.EventAPI.Repositories.Interface;
using GreenCorner.EventAPI.Services.Interface;

namespace GreenCorner.EventAPI.Services
{
    public class TrashEventService : ITrashEventService
    {
        private readonly ITrashEventRepository _trashEventRepository;
        private readonly IMapper _mapper;

        public TrashEventService(ITrashEventRepository trashEventRepository, IMapper mapper)
        {
            _trashEventRepository = trashEventRepository;
            _mapper = mapper;
        }
        public async Task AddTrashEvent(TrashEventDTO trashEventDTO)
        {
            TrashEvent trashEvent = _mapper.Map<TrashEvent>(trashEventDTO);
            await _trashEventRepository.AddTrashEvent(trashEvent);
        }

        public async Task DeleteTrashEvent(int id)
        {
            await _trashEventRepository.DeleteTrashEvent(id);
        }

        public async Task<IEnumerable<TrashEventDTO>> GetAllTrashEvent()
        {
            var trashEvents = await _trashEventRepository.GetAllTrashEvent();
            return _mapper.Map<List<TrashEventDTO>>(trashEvents);
        }

        public async Task<TrashEventDTO> GetByTrashEventId(int id)
        {
            var trashEvent = await _trashEventRepository.GetByTrashEventId(id);
            return _mapper.Map<TrashEventDTO>(trashEvent);
        }

        public Task UpdateTrashEvent(TrashEventDTO TrashEventDTO)
        {
            TrashEvent trashEvent = _mapper.Map<TrashEvent>(TrashEventDTO);
            return _trashEventRepository.UpdateTrashEvent(trashEvent);
        }
    }
}
