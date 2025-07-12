using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GreenCorner.MVC.Controllers
{
    public class RewardController : Controller
    {
        private readonly IRewardService _rewardService;
        public RewardController(IRewardService rewardService)
        {
            _rewardService = rewardService;
        }
        public async Task<IActionResult> Index()
        {
            List<VoucherDTO> listVoucher = new();
            ResponseDTO? response = await _rewardService.GetAllReward();
            if (response != null && response.IsSuccess)
            {
                listVoucher = JsonConvert.DeserializeObject<List<VoucherDTO>>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response?.Message;
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
