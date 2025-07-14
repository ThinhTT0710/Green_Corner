using GreenCorner.RewardAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.RewardAPI.Services.Interface
{
    public interface IPointTransactionService
    {
        Task<IEnumerable<PointTransactionDTO>> GetAllPointTransaction();
        Task<IEnumerable<PointTransactionDTO>> GetByUserId(string userId);
        Task TransactionPoint(string userId, int points);
        Task<PointTransactionDTO> GetPointTransaction(string userId);
    }

}
