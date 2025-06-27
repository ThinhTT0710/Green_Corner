using AutoMapper;
using GreenCorner.RewardAPI.Models;
using GreenCorner.RewardAPI.Models.DTO;
using GreenCorner.RewardAPI.Repositories;
using GreenCorner.RewardAPI.Repositories.Interface;
using GreenCorner.RewardAPI.Services.Interface;

namespace GreenCorner.RewardAPI.Services
{
    public class PointTransactionService : IPointTransactionService
    {
        private readonly IPointTransactionRepository _pointTransactionRepository;
        private readonly IMapper _mapper;

        public PointTransactionService(IPointTransactionRepository pointTransactionRepository, IMapper mapper)
        {
            _pointTransactionRepository = pointTransactionRepository;
            _mapper = mapper;
        }

        public async Task<PointTransactionDTO> GetPointTransaction(string userId)
        {
            var pointTransaction = await _pointTransactionRepository.GetPointTransaction(userId);
            return _mapper.Map<PointTransactionDTO>(pointTransaction);
        }

        public async Task TransactionPoint(string userId, int points)
        {
            await _pointTransactionRepository.TransactionPoint(userId, points);
        }
    }
}
