using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace GreenCorner.MVC.Controllers
{
    public class RewardRedemptionHistoryController : Controller
    {
        private readonly IRewardRedemptionHistoryService _rewardRedemptionHistoryService;
        private readonly IVoucherService _voucherService;

        public RewardRedemptionHistoryController(IRewardRedemptionHistoryService rewardRedemptionHistoryService, IVoucherService voucherService)
        {
            _rewardRedemptionHistoryService = rewardRedemptionHistoryService;
            _voucherService = voucherService;
        }
        public async Task<IActionResult> GetRewardRedemptionHistory()
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;

            var response = await _rewardRedemptionHistoryService.GetRewardRedemptionHistory(userId);

            if (response != null && response.IsSuccess)
            {
                var redemptionList = JsonConvert.DeserializeObject<List<UserVoucherRedemptionDTO>>(response.Result?.ToString());
                var result = new List<UserVoucherRedemptionViewModel>();

                foreach (var redemption in redemptionList!)
                {
                    var voucherRes = await _voucherService.GetVoucherById(redemption.VoucherId);
                    var voucher = voucherRes != null && voucherRes.IsSuccess
                        ? JsonConvert.DeserializeObject<VoucherDTO>(voucherRes.Result?.ToString()!)
                        : new VoucherDTO();

                    result.Add(new UserVoucherRedemptionViewModel
                    {
                        Redemption = redemption,
                        Voucher = voucher!
                    });
                }

                return View(result);
            }

            TempData["error"] = response?.Message ?? "Không thể tải lịch sử đổi điểm.";
            return View(new List<UserVoucherRedemptionViewModel>());
        }


    }
}
