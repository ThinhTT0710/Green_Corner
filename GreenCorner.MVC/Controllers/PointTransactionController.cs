using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace GreenCorner.MVC.Controllers
{
    public class PointTransactionController : Controller
    {
        private readonly IPointTransactionService _pointTransactionService;
        private readonly IUserService _userService;

        public PointTransactionController(IPointTransactionService pointTransactionService, IUserService userService)
        {
            _pointTransactionService = pointTransactionService;
            _userService = userService;
        }
        public async Task<IActionResult> PointTransaction()
        {
            return View();
        }
        public async Task<IActionResult> PointTransactionList()
        {
            List<PointTransactionListViewModel> viewModelList = new();
            ResponseDTO? response = await _pointTransactionService.GetAllPointTransactions();
            if (response != null && response.IsSuccess)
            {
                var pointTransactions = JsonConvert.DeserializeObject<List<PointTransactionDTO>>(response.Result.ToString());
                foreach (var pointTransaction in pointTransactions)
                {
                    ResponseDTO? userResponse = await _userService.GetUserById(pointTransaction.UserId);

                    UserDTO userDTO = new UserDTO();

                    if (userResponse != null && userResponse.IsSuccess)
                    {
                        userDTO = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result.ToString());
                    }

                    viewModelList.Add(new PointTransactionListViewModel
                    {
                        pointTransactionDTO = pointTransaction,
                        User = userDTO
                    });
                }
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(viewModelList);
        }

        [HttpPost]
        public async Task<IActionResult> PointTransaction(string userId, int exchangePoint)
        {
            var response = await _pointTransactionService.ExchangePoints(userId, exchangePoint);
            if (response != null && response.IsSuccess)
                return RedirectToAction("Index","RewardPoint");

            return View("Error"); 
        }

    
        public async Task<IActionResult> ViewTransactions(string userId)
        {
            var response = await _pointTransactionService.GetExchangeTransactions(userId);
            return View(response);
        }

		[HttpPost]
		public async Task<IActionResult> EarnPoints(string userId, int points)
		{
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Bạn cần đăng nhập để thực hiện thao tác này.";
                return RedirectToAction("Login", "Auth");
            }
            var dto = new PointTransactionDTO
			{
				UserId = userId,
				Points = points,
				Type = "Thưởng"
            };

			var response = await _pointTransactionService.TransactionPoints(dto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = $"Bạn đã được Thưởng {points} điểm thành công.";
                return RedirectToAction("Index", "Voucher");
            }

            TempData["error"] = "Đổi điểm thất bại. Vui lòng thử lại.";
            return RedirectToAction("Index", "Voucher");
        }

		[HttpPost]
		public async Task<IActionResult> ExchangePoints(int points)
		{
			var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Bạn cần đăng nhập để thực hiện thao tác này.";
                return RedirectToAction("Login", "Auth");
            }
            var dto = new PointTransactionDTO
			{
				UserId = userId,
				Points = points,
				Type = "Đổi"
			};

			var response = await _pointTransactionService.TransactionPoints(dto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = $"Bạn đã đổi {points} điểm thành công.";
                return RedirectToAction("Index", "Voucher"); 
            }

            TempData["error"] = "Đổi điểm thất bại. Vui lòng thử lại.";
            return RedirectToAction("Index", "Voucher");
        }

        public async Task<IActionResult> UserTransactions()
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Bạn cần đăng nhập để xem lịch sử giao dịch.";
                return RedirectToAction("Login", "Auth");
            }

            var response = await _pointTransactionService.GetUserPointTransactions(userId);

            if (response != null && response.IsSuccess)
            {
                var transactionList = JsonConvert.DeserializeObject<List<PointTransactionDTO>>(response.Result.ToString());
                return View(transactionList);
            }

            TempData["error"] = "Không thể tải lịch sử giao dịch.";
            return View(new List<PointTransactionDTO>());
        }
    }

}
