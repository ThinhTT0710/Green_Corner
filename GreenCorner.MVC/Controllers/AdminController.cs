using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GreenCorner.MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
		private readonly IAdminService _adminService;

		public AdminController(IUserService userService, IAdminService adminService)
		{
			_userService = userService;
			_adminService = adminService;
		}

		public IActionResult Index()
        {
            return View();
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
				var rawLogs = JsonSerializer.Deserialize<List<SystemLogDTO>>(response.Result.ToString(),
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
