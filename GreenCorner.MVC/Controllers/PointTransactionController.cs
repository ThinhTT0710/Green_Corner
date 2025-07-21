using GreenCorner.MVC.Models;
using GreenCorner.MVC.Models.Notification;
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
        private readonly IRewardRedemptionHistoryService _rewardRedemptionHistoryService;
        private readonly IUserService _userService;
        private readonly IVoucherService _voucherService;
        private readonly INotificationService _notificationService;
        public PointTransactionController(IPointTransactionService pointTransactionService, IRewardRedemptionHistoryService rewardRedemptionHistory, IUserService userService, IVoucherService voucherService, INotificationService notificationService)
        {
            _pointTransactionService = pointTransactionService;
            _rewardRedemptionHistoryService = rewardRedemptionHistory;
            _userService = userService;
            _voucherService = voucherService;
            _notificationService = notificationService;
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
        public async Task<IActionResult> EarnPoints(string userId, int points, int eventId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Bạn cần đăng nhập để thực hiện thao tác này.";
                return RedirectToAction("Login", "Auth");
            }

            // 🔍 Kiểm tra xem đã được thưởng điểm cho sự kiện này chưa
            var check = await _pointTransactionService.HasReceivedReward(userId, eventId);
            if (check != null && check.IsSuccess && (bool)(check.Result ?? false))
            {
                TempData["error"] = "Bạn đã nhận điểm cho sự kiện này rồi.";
                return RedirectToAction("ViewEventVolunteerList", "Event", new { eventId = eventId });

            }

            // 🏆 Nếu chưa được nhận điểm thì tiến hành thưởng
            var dto = new PointTransactionDTO
            {
                UserId = userId,
                Points = points,
                Type = "Thưởng",
                CleanEventId = eventId // Nếu DTO của bạn có thuộc tính này
            };

            var response = await _pointTransactionService.TransactionPoints(dto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = $"Bạn đã được Thưởng {points} điểm thành công.";
                return RedirectToAction("ViewEventVolunteerList", "Event", new { eventId = eventId });

            }

            TempData["error"] = "Thưởng điểm thất bại. Vui lòng thử lại.";
            return RedirectToAction("ViewEventVolunteerList", "Event", new { eventId = eventId });
        }


        [HttpPost]
		public async Task<IActionResult> ExchangePoints(int points, int voucherId)
		{
			var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Bạn cần đăng nhập để thực hiện thao tác này.";
                return RedirectToAction("Login", "Auth");
            }

            var redeemResponse = await _voucherService.RedeemVoucher(voucherId);
            if (redeemResponse == null || !redeemResponse.IsSuccess)
            {
                TempData["error"] = "Voucher không còn khả dụng hoặc đã hết.";
                return RedirectToAction("Index", "Reward");
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
                var redemptionResponse = await _rewardRedemptionHistoryService.SaveRedemptionAsync(userId, voucherId);

                if (redemptionResponse != null && redemptionResponse.IsSuccess)
                {
                    var voucherInfo = await _voucherService.GetVoucherById(voucherId);
                    var voucherDto = JsonConvert.DeserializeObject<VoucherDTO>(voucherInfo.Result?.ToString() ?? "");
                    string voucherCode = voucherDto?.Code ?? "(không rõ mã)";
                    var notification = new NotificationDTO
                    {
                        UserId = userId,
                        Title = "Đổi điểm thành công",
                        Message = $"Bạn đã đổi {points} điểm để nhận voucher: {voucherCode}."
                    };

                    var sendNotification = await _notificationService.SendNotification(notification);

                    if (sendNotification != null && sendNotification.IsSuccess)
                    {
                        TempData["success"] = $"Bạn đã đổi {points} điểm và nhận voucher thành công. Đã gửi thông báo!";
                    }
                    else
                    {
                        TempData["warning"] = $"Đổi điểm thành công, nhưng gửi thông báo thất bại: {sendNotification?.Message}";
                    }
                }
                else
                {
                    TempData["warning"] = $"Đổi điểm thành công, nhưng ghi nhận voucher thất bại: {redemptionResponse?.Message}";
                }

                return RedirectToAction("Index", "Reward");
            }

            TempData["error"] = response?.Message ?? "Đổi điểm thất bại. Vui lòng thử lại.";
            return RedirectToAction("Index", "Reward");
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
