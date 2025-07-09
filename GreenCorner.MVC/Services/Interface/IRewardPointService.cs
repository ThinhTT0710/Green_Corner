using GreenCorner.MVC.Models;
using System.Threading.Tasks;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IRewardPointService
    {
        Task<ResponseDTO?> GetRewardPoints(); 
        Task<ResponseDTO?> AwardPointsToUser(string userId, int points); 
        Task<ResponseDTO?> GetUserTotalPoints(string userId);
    }
}