using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
	public interface IAdminService
	{
		Task<ResponseDTO?> AddLogStaff(SystemLogDTO logDTO);
		Task<ResponseDTO?> GetAllLog();
        Task<ResponseDTO?> GetMonthlyAnalytics();
        Task<ResponseDTO?> EventMonthlyAnalytics();
        Task<ResponseDTO?> GetSalesByCategory();
    }
}
