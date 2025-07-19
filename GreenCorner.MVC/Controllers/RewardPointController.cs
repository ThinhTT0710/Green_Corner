using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace GreenCorner.MVC.Controllers
{
    public class RewardPointController : Controller
    {
        private readonly IRewardPointService _rewardPointService;
        private readonly IUserService _userService;

        public RewardPointController(IRewardPointService rewardPointService, IUserService userService)
        {
            _rewardPointService = rewardPointService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            List<RewardPointWithUserViewModel> viewModels = new();

            ResponseDTO? response = await _rewardPointService.GetRewardPoints();
            if (response != null && response.IsSuccess)
            {
                var rewardList = JsonConvert.DeserializeObject<List<RewardPointDTO>>(response.Result.ToString());

                foreach (var reward in rewardList)
                {
                    UserDTO? user = null;
                    if (!string.IsNullOrEmpty(reward.UserId))
                    {
                        var userResponse = await _userService.GetUserById(reward.UserId);
                        if (userResponse != null && userResponse.IsSuccess)
                        {
                            user = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result.ToString());
                        }
                    }

                    viewModels.Add(new RewardPointWithUserViewModel
                    {
                        Reward = reward,
                        User = user
                    });
                }
            }

            return View(viewModels);
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
        public async Task<IActionResult> GetTotalRewardPoints()
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
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