using GreenCorner.RewardAPI.Models.DTO;

namespace GreenCorner.RewardAPI.Services.Interface
{
    public interface IPointTransactionService
    {
        Task TransactionPoint(string userId, int points);
        Task<PointTransactionDTO> GetPointTransaction(string userId);
    }

}
