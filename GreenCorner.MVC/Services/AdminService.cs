using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;

namespace GreenCorner.MVC.Services
{
	public class AdminService : IAdminService
	{
		private readonly IBaseService _baseService;

		public AdminService(IBaseService baseService)
		{
			_baseService = baseService;
		}

		public async Task<ResponseDTO?> GetAllLog()
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				APIType = SD.APIType.GET,
				Url = SD.AuthAPIBase + "/api/Admin/get-all-log"
			});
		}

		public async Task<ResponseDTO?> AddLogStaff(SystemLogDTO logDTO)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				APIType = SD.APIType.POST,
				Data = logDTO,
				Url = SD.AuthAPIBase + "/api/Admin/add-log-staff"
			});
		}

        public async Task<ResponseDTO?> GetMonthlyAnalytics()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EcommerceAPIBase + "/api/order/monthly-analytics"
            });
        }

        public async Task<ResponseDTO?> EventMonthlyAnalytics()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EventAPIBase + "/api/trashevent/monthly-analytics"
            });
        }

        public async Task<ResponseDTO?> GetSalesByCategory()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EcommerceAPIBase + "/api/order/sales-by-category"
            });
        }
    }
}
