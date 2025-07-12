using GreenCorner.MVC.Models;
using GreenCorner.MVC.Utility;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IRewardService
    {
        Task<ResponseDTO?> GetAllReward();
        Task<ResponseDTO?> GetVoucherById(int id);

    }
}
   
