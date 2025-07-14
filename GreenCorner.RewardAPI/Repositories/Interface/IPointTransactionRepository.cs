using GreenCorner.RewardAPI.Models;

namespace GreenCorner.RewardAPI.Repositories.Interface
{
    public interface IPointTransactionRepository
    {
        Task<IEnumerable<PointTransaction>> GetAllPointTransaction();
        Task<IEnumerable<PointTransaction>> GetByUserId(string userId);
        Task TransactionPoint(string userId, int points);
        Task<PointTransaction> GetPointTransaction(string userId);
    }

}


