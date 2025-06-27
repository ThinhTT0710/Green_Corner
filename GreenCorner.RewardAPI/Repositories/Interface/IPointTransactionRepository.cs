using GreenCorner.RewardAPI.Models;

namespace GreenCorner.RewardAPI.Repositories.Interface
{
    public interface IPointTransactionRepository
    {
        Task TransactionPoint(string userId, int points);
        Task<PointTransaction> GetPointTransaction(string userId);
    }

}


