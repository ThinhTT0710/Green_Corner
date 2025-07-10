using GreenCorner.MVC.Models;
using GreenCorner.MVC.Models.Admin;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;

namespace GreenCorner.MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService; 
		private readonly IAdminService _adminService; 
		private readonly IOrderService _orderService;
		private readonly IProductService _productservice;

        public AdminController(IUserService userService, IAdminService adminService, IOrderService orderService, IProductService productservice)
        {
            _userService = userService;
            _adminService = adminService;
            _orderService = orderService;
            _productservice = productservice;
        }

        public async Task<IActionResult> Index()
        {
            int totalOrderComplete = 0;
            int totalOrderWaiting = 0;
			int totalSales = 0;
			int totalMoneyInMonth = 0;

            MonthlyAnalyticsDto chartData = new();
			CategorySalesDto categorySalesData = new CategorySalesDto();
			List<BestSellingProductDTO> bestSelling = new();
			List<ProductDTO> outOfStockProducts = new();

            ResponseDTO? responseOrderComplete = await _orderService.TotalOrdersComplete();
            if (responseOrderComplete != null && responseOrderComplete.IsSuccess)
            {
                totalOrderComplete = Convert.ToInt32(responseOrderComplete.Result);
            }
            else
            {
                TempData["error"] = responseOrderComplete.Message == null ? "Error" : responseOrderComplete.Message;
                return RedirectToAction("Index", "Admin");
            }
            ResponseDTO? responseOrderWaiting = await _orderService.TotalOrdersWaiting();
			if (responseOrderWaiting != null && responseOrderWaiting.IsSuccess)
			{
				totalOrderWaiting = Convert.ToInt32(responseOrderWaiting.Result);
			}
			else
			{
				TempData["error"] = responseOrderWaiting.Message == null ? "Error" : responseOrderWaiting.Message;
				return RedirectToAction("Index", "Admin");
			}
			ResponseDTO? responseTotalSales = await _orderService.TotalSales();
			if (responseTotalSales != null && responseTotalSales.IsSuccess)
			{
				totalSales = Convert.ToInt32(responseTotalSales.Result);
			}
			else
			{
				TempData["error"] = responseTotalSales.Message == null ? "Error" : responseTotalSales.Message;
				return RedirectToAction("Index", "Admin");
			}
			ResponseDTO? responseTotalMoney = await _orderService.GetTotalMoneyByMonth();
			if (responseTotalMoney != null && responseTotalMoney.IsSuccess)
			{
				totalMoneyInMonth = Convert.ToInt32(responseTotalMoney.Result);
			}
			else
			{
				TempData["error"] = responseTotalMoney.Message == null ? "Error" : responseTotalMoney.Message;
				return RedirectToAction("Index", "Admin");
			}
			ResponseDTO? responseChart = await _adminService.GetMonthlyAnalytics();
            if (responseChart != null && responseChart.IsSuccess)
            {
                chartData = JsonConvert.DeserializeObject<MonthlyAnalyticsDto>(Convert.ToString(responseChart.Result));
            }
            else
            {
                TempData["error"] = responseChart?.Message ?? "Error fetching chart data";
            }
            ResponseDTO? responseCategorySales = await _adminService.GetSalesByCategory();
            if (responseCategorySales != null && responseCategorySales.IsSuccess)
            {
                categorySalesData = JsonConvert.DeserializeObject<CategorySalesDto>(Convert.ToString(responseCategorySales.Result));
            }
            else
            {
                TempData["error"] = responseCategorySales?.Message ?? "Error fetching category sales data";
            }
            ResponseDTO? responseBestSeller = await _orderService.GetBestSellingProduct();
            if (responseBestSeller != null && responseBestSeller.IsSuccess)
            {
                bestSelling = JsonConvert.DeserializeObject<List<BestSellingProductDTO>>(responseBestSeller.Result.ToString());
            }
            else
            {
                TempData["error"] = responseBestSeller.Message == null ? "Error" : responseBestSeller.Message;
                return RedirectToAction("Index", "Admin");
            }
            ResponseDTO? responseOutOfStock = await _productservice.OutOfStockProduct();
            if (responseOutOfStock != null && responseOutOfStock.IsSuccess)
            {
                outOfStockProducts = JsonConvert.DeserializeObject<List<ProductDTO>>(responseOutOfStock.Result.ToString());
            }
            else
            {
                TempData["error"] = responseBestSeller.Message == null ? "Error" : responseOutOfStock.Message;
                return RedirectToAction("Index", "Admin");
            }
            var viewModel = new SaleAnalytics
			{
				TotalOrdersComplete = totalOrderComplete,
				TotalOrdersWaiting = totalOrderWaiting,
				TotalSales = totalSales,
				TotalMoneyByMonth = totalMoneyInMonth,
				BestSellingProducts = bestSelling,
                OutOfStockProduct = outOfStockProducts,
                ChartData = chartData,
                CategorySalesChartData = categorySalesData
            };

			return View(viewModel);
        }

        public async Task<IActionResult> UserList()
        {
            var response = await _userService.GetAllUser();
            if (response != null && response.IsSuccess)
            {
                List<UserDTO> users = response.Result != null ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserDTO>>(response.Result.ToString()) : new List<UserDTO>();
                return View(users);
            }
            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> BlockUser(string userId)
        { 
			try
			{
				var response = await _userService.BanUser(userId);
				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Chặn user thành công";
				}
				else
				{
					TempData["error"] = response?.Message;
				}
				return RedirectToAction("UserList", "Admin");
			}
			catch (Exception ex)
			{
				TempData["error"] = ex.Message;
				return RedirectToAction("UserList", "Admin");
			}
		}

        public async Task<IActionResult> UnblockUser(string userId)
        {
			try
			{
				var response = await _userService.UnBanUser(userId);
				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Mở chặn user thành công";
				}
				else
				{
					TempData["error"] = response?.Message;
				}
				return RedirectToAction("UserList", "Admin");
			}
			catch (Exception ex)
			{
				TempData["error"] = ex.Message;
				return RedirectToAction("UserList", "Admin");
			}
		}

		public async Task<IActionResult> LogStaff()
		{
			var response = await _adminService.GetAllLog();
			if (response != null && response.IsSuccess)
			{
				var rawLogs = System.Text.Json.JsonSerializer.Deserialize<List<SystemLogDTO>>(response.Result.ToString(),
			new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

				var viewModel = rawLogs.Select(log => new SystemLogViewModel
				{
					Id = log.Id,
					UserId = log.UserId,
					ActionType = log.ActionType,
					ObjectType = log.ObjectType,
					ObjectId = log.ObjectId,
					Description = log.Description,
					CreatedAt = log.CreatedAt,
					FullName = log.User?.FullName,
					Address = log.User?.Address,
					Avatar = log.User?.Avatar ?? "default.png",
					UserName = log.User?.UserName,
					Email = log.User?.Email,
					IsBanned = log.User?.LockoutEnd != null && log.User.LockoutEnd > DateTimeOffset.UtcNow
				}).ToList();

				return View(viewModel);
			}
			return RedirectToAction("Index", "Admin");
		}

	}
}
