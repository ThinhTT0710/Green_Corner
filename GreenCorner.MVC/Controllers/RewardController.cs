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

    }
}
