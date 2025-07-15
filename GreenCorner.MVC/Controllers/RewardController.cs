using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace GreenCorner.MVC.Controllers
{
    public class RewardController : Controller
    {
        private readonly IRewardService _rewardService;
        private readonly IRewardPointService _rewardPointService;
        public RewardController(IRewardService rewardService, IRewardPointService rewardPointService)
        {
            _rewardService = rewardService;
            _rewardPointService = rewardPointService;
        }
        public async Task<IActionResult> Index()
        {
            List<VoucherDTO> listVoucher = new();
            ResponseDTO? response = await _rewardService.GetAllReward();

            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;

            if (response != null && response.IsSuccess)
            {
                listVoucher = JsonConvert.DeserializeObject<List<VoucherDTO>>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            // Lấy tổng điểm của người dùng
            if (!string.IsNullOrEmpty(userId))
            {
                var pointResponse = await _rewardPointService.GetUserTotalPoints(userId);
                if (pointResponse != null && pointResponse.IsSuccess)
                {
                    var rewardPoint = JsonConvert.DeserializeObject<RewardPointDTO>(pointResponse.Result.ToString());
                    ViewBag.TotalPoints = rewardPoint.TotalPoints;
                }
                else
                {
                    ViewBag.TotalPoints = 0;
                }
            }
            else
            {
                ViewBag.TotalPoints = 0;
            }

            return View(listVoucher);
        }

        public async Task<IActionResult> ViewDetailVoucher(int voucherId)
        {
            VoucherDTO voucher = new();
            ResponseDTO? response = await _rewardService.GetVoucherById(voucherId);

            if (response != null && response.IsSuccess)
            {
                voucher = JsonConvert.DeserializeObject<VoucherDTO>(response.Result.ToString());
            }
            else
            {
                return NotFound(); // hoặc RedirectToAction("Index") nếu bạn muốn
            }

            return View("Detail", voucher); // View tên Detail.cshtml
        }

    }
}
