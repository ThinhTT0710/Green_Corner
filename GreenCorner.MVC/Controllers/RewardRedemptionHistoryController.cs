using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace GreenCorner.MVC.Controllers
{
    public class RewardRedemptionHistoryController : Controller
    {
        private readonly IRewardRedemptionHistoryService _rewardRedemptionHistoryService;

        public RewardRedemptionHistoryController(IRewardRedemptionHistoryService rewardRedemptionHistoryService)
        {
            _rewardRedemptionHistoryService = rewardRedemptionHistoryService;
        }
        public async Task<IActionResult> GetRewardRedemptionHistory()
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;

            ResponseDTO response = await _rewardRedemptionHistoryService.GetRewardRedemptionHistory(userId);

            if (response != null && response.IsSuccess)
            {
                var listHistory = JsonConvert.DeserializeObject<List<PointTransactionDTO>>(response.Result.ToString());
                return View(listHistory);
            }

            TempData["error"] = response?.Message ?? "Cannot load reward redemption history.";
            return View(new List<PointTransactionDTO>()); // trả về list rỗng
        }

    }
}
