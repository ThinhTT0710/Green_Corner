using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace GreenCorner.MVC.Controllers
{
    public class RewardPointController : Controller
    {
        private readonly IRewardPointService _rewardPointService;

        public RewardPointController(IRewardPointService rewardPointService)
        {
            _rewardPointService = rewardPointService;
        }

        public async Task<IActionResult> Index()
        {
            List<RewardPointDTO> listReward = new();
            ResponseDTO? response = await _rewardPointService.GetRewardPoints();
            if (response != null && response.IsSuccess)
            {
                listReward = JsonConvert.DeserializeObject<List<RewardPointDTO>>(response.Result.ToString());
            }
            return View(listReward);
        }


        [HttpGet]
        public async Task<IActionResult> AwardPoints(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            }

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            ResponseDTO response = await _rewardPointService.GetUserTotalPoints(userId);

            if (response != null && response.IsSuccess)
            {
                var rewardPointDTO = JsonConvert.DeserializeObject<RewardPointDTO>(response.Result.ToString());

                if (rewardPointDTO != null)
                {
                    return View(rewardPointDTO);
                }
            }

            return NotFound();
        }



        [HttpPost]
        public async Task<IActionResult> AwardPoints(string UserId, int TotalPoints)
        {
            ResponseDTO response = await _rewardPointService.AwardPointsToUser(UserId, TotalPoints);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Awarded successfully!";
                return RedirectToAction("Index");
            }

            TempData["error"] = response?.Message ?? "Failed to award points.";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> GetTotalRewardPoints(string userId)
        {
            ResponseDTO response = await _rewardPointService.GetUserTotalPoints(userId);
            if (response != null && response.IsSuccess)
            {
                RewardPointDTO rewardPointDTO = JsonConvert.DeserializeObject<RewardPointDTO>(response.Result.ToString());
                return View(rewardPointDTO);
            }
            return NotFound();
        }
    }
}