using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace GreenCorner.MVC.Controllers
{
    public class RewardHistoryController : Controller
    {
        private readonly IRewardRedemptionHistoryService _rewardRedemptionHistoryService;
        private readonly IVoucherService _voucherService;
        private readonly IUserService _userService;

        public RewardHistoryController(IRewardRedemptionHistoryService rewardRedemptionHistoryService, IVoucherService voucherService, IUserService userService)
        {
            _rewardRedemptionHistoryService = rewardRedemptionHistoryService;
            _voucherService = voucherService;
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> ViewUsersRedeemedReward()
        {
            var response = await _rewardRedemptionHistoryService.GetUserRewardRedemption();

            if (response == null || !response.IsSuccess)
            {
                TempData["error"] = response?.Message ?? "Không thể lấy danh sách người dùng đã đổi thưởng.";
                return RedirectToAction("Index", "Home");
            }

            var userIds = JsonConvert.DeserializeObject<List<string>>(response.Result.ToString());

            if (userIds == null || !userIds.Any())
            {
                TempData["error"] = "Không có người dùng nào đã đổi thưởng.";
                return RedirectToAction("Index", "Home");
            }

            var viewModelList = new List<UserWithVoucherRedemptionViewModel>();

            foreach (var userId in userIds.Distinct()) 
            {
                var userResponse = await _userService.GetUserById(userId);

                if (userResponse != null && userResponse.IsSuccess)
                {
                    var user = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result?.ToString() ?? "");

                    viewModelList.Add(new UserWithVoucherRedemptionViewModel
                    {
                        User = user
                    });
                }
                else
                {
                    TempData["warning"] = $"Không thể lấy thông tin người dùng có ID: {userId}";
                }
            }

            return View(viewModelList);
        }
        [HttpPost]
        public async Task<IActionResult> MarkAsUsed(int userVoucherId, string userId)
        {
            var response = await _rewardRedemptionHistoryService.MarkAsUsedAsync(userVoucherId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Sử dụng Voucher thành công.";
            }
            else
            {
                TempData["error"] = response?.Message ?? "Sử dụng Voucher thất bại!";
            }
            return RedirectToAction("GetUserRewardRedemptionHistory", "User", new { userId });
        }
    }
}
