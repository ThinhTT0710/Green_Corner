using AutoMapper;
using GreenCorner.RewardAPI.Models;
using GreenCorner.RewardAPI.Models.DTO;
using GreenCorner.RewardAPI.Repositories;
using GreenCorner.RewardAPI.Repositories.Interface;
using GreenCorner.RewardAPI.Services.Interface;

namespace GreenCorner.RewardAPI.Services
{
    public class RewardPointService : IRewardPointService
    {
        private readonly IRewardPointRepository _rewardPointRepository;
        private readonly IMapper _mapper;

        public RewardPointService(IRewardPointRepository rewardPointRepository, IMapper mapper)
        {
            _rewardPointRepository = rewardPointRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RewardPointDTO>> GetRewardPoints()
        {

            var reward = await _rewardPointRepository.GetRewardPoints();
            return _mapper.Map<List<RewardPointDTO>>(reward);
        }
        public async Task AwardPointsToUser(string userId, int points)
        {
            await _rewardPointRepository.AwardPointsToUser(userId,points);
        }
       

        public async Task<RewardPointDTO> GetUserTotalPoints(string userId)
        {
            var voucher = await _rewardPointRepository.GetUserTotalPoints(userId);
            return _mapper.Map<RewardPointDTO>(voucher);

        }
    }

}