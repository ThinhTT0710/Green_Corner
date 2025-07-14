
﻿using GreenCorner.RewardAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
﻿using GreenCorner.RewardAPI.Models;

namespace GreenCorner.RewardAPI.Services.Interface
{
    public interface IPointTransactionService
    {
        Task<IEnumerable<PointTransactionDTO>> GetAllPointTransaction();
        Task<IEnumerable<PointTransactionDTO>> GetByUserId(string userId);
        Task TransactionPoint(string userId, int points);
        Task<PointTransactionDTO> GetPointTransaction(string userId);

		Task TransactionPoints(string userId, int points, string type);
		Task<IEnumerable<PointTransactionDTO>> GetRewardPointByUserIdAsync(string userId);
		Task AddTransactionAsync(PointTransactionDTO transaction);
		Task UpdateRewardPointAsync(RewardPointDTO rewardPoint);
        Task<IEnumerable<PointTransactionDTO>> GetPointsAwardHistoryAsync();
    }

}
