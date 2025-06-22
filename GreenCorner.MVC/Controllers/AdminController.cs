using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
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
    }
}
