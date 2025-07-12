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

		public async Task AddTransactionAsync(PointTransactionDTO transaction)
		{
			var transactionDto = _mapper.Map<PointTransaction>(transaction);
			await _pointTransactionRepository.AddTransactionAsync(transactionDto);
		}

        public async Task<IEnumerable<PointTransactionDTO>> GetPointsAwardHistoryAsync()
        {
            var transactions = await _pointTransactionRepository.GetPointsAwardHistoryAsync();
            return _mapper.Map<IEnumerable<PointTransactionDTO>>(transactions);
        }

        public async Task<PointTransactionDTO> GetPointTransaction(string userId)
        {
            var pointTransaction = await _pointTransactionRepository.GetPointTransaction(userId);
            return _mapper.Map<PointTransactionDTO>(pointTransaction);
        }

		public async Task<IEnumerable<PointTransactionDTO>> GetRewardPointByUserIdAsync(string userId)
		{
			var transactions = await _pointTransactionRepository.GetRewardPointByUserIdAsync(userId);
			return _mapper.Map<IEnumerable<PointTransactionDTO>>(transactions);
		}

		public async Task TransactionPoint(string userId, int points)
        {
            await _pointTransactionRepository.TransactionPoint(userId, points);
        }

		public async Task TransactionPoints(string userId, int points, string type)
		{
			await _pointTransactionRepository.TransactionPoints(userId, points, type);
		}

		public async Task UpdateRewardPointAsync(RewardPointDTO rewardPoint)
		{
			var rewardPointDto = _mapper.Map<RewardPoint>(rewardPoint);
			await _pointTransactionRepository.UpdateRewardPointAsync(rewardPointDto);
		}
	}
}
