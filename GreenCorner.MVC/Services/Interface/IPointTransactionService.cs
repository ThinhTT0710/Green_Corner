using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IPointTransactionService
    {
        Task<ResponseDTO?> ExchangePoints(string userId, int exchangePoint); 
        Task<ResponseDTO?> GetExchangeTransactions(string userId); 
        Task<ResponseDTO?> GetUserRewardPoints(string userId); 
        Task<ResponseDTO?> AwardRewardPoints(RewardPointDTO rewardPointDTO); 
        Task<ResponseDTO?> GetTotalRewardPoints(string userId);

		Task<ResponseDTO?> TransactionPoints(PointTransactionDTO dto);
		Task<ResponseDTO?> GetUserPointTransactions(string userId);
        Task<ResponseDTO?> GetPointsAwardHistoryAsync();
    }


}