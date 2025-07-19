using AutoMapper;
using GreenCorner.RewardAPI.Models;
using GreenCorner.RewardAPI.Models.DTO;
using GreenCorner.RewardAPI.Repositories;
using GreenCorner.RewardAPI.Repositories.Interface;
using GreenCorner.RewardAPI.Services.Interface;

namespace GreenCorner.RewardAPI.Services
{
    public class RewardRedemptionHistoryService : IRewardRedemptionHistoryService
    {
        private readonly IRewardRedemptionHistoryRepository _rewardRedemptionHistoryRepository;
        private readonly IMapper _mapper;

        public RewardRedemptionHistoryService(IRewardRedemptionHistoryRepository rewardRedemptionHistoryRepository, IMapper mapper)
        {
            _rewardRedemptionHistoryRepository = rewardRedemptionHistoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserVoucherRedemptionDTO>> GetRewardRedemptionHistory(string userId)
        {
            var pointTransactions = await _rewardRedemptionHistoryRepository.GetRewardRedemptionHistory(userId);
            return _mapper.Map<List<UserVoucherRedemptionDTO>>(pointTransactions);
        }

        public async Task SaveRedemptionAsync(string userId, int voucherId)
        {
            await _rewardRedemptionHistoryRepository.SaveRedemptionAsync(userId, voucherId);
        }

        public async Task<IEnumerable<string>> GetDistinctUserIdsRedeemedAsync()
        {
            return await _rewardRedemptionHistoryRepository.GetDistinctUserIdsRedeemedAsync();
        }

        public async Task<UserVoucherRedemptionDTO> UpdateIsUsedAsync(int userVoucherId)
        {
            var updatedRedemption = await _rewardRedemptionHistoryRepository.UpdateIsUsedAsync(userVoucherId);

            if (updatedRedemption == null)
                throw new Exception("Không tìm thấy bản ghi hoặc cập nhật thất bại.");

            return _mapper.Map<UserVoucherRedemptionDTO>(updatedRedemption);
        }
    }
}
