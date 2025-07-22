using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
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

        [HttpPost]
        public async Task<IActionResult> ExportToExcel(string userId)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var response = await _rewardRedemptionHistoryService.GetRewardRedemptionHistory(userId);
            if (response == null || !response.IsSuccess || string.IsNullOrEmpty(response.Result?.ToString()))
            {
                TempData["error"] = "Không có dữ liệu để xuất.";
                return RedirectToAction("GetUserRewardRedemptionHistory", "User", new { userId });
            }

            var redemptionList = JsonConvert.DeserializeObject<List<UserVoucherRedemptionDTO>>(response.Result?.ToString());
            var viewModels = new List<UserVoucherRedemptionViewModel>();

            // Lấy user một lần duy nhất
            var userRes = await _userService.GetUserById(userId);
            var user = userRes != null && userRes.IsSuccess
                ? JsonConvert.DeserializeObject<UserDTO>(userRes.Result?.ToString())
                : new UserDTO();

            foreach (var redemption in redemptionList!)
            {
                var voucherRes = await _voucherService.GetVoucherById(redemption.VoucherId);
                var voucher = voucherRes != null && voucherRes.IsSuccess
                    ? JsonConvert.DeserializeObject<VoucherDTO>(voucherRes.Result?.ToString())
                    : new VoucherDTO();

                viewModels.Add(new UserVoucherRedemptionViewModel
                {
                    Redemption = redemption,
                    Voucher = voucher,
                    User = user
                });
            }

            // Bắt đầu export Excel
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Lịch sử đổi điểm");

            worksheet.Cells[1, 1].Value = "Mã đổi";
            worksheet.Cells[1, 2].Value = "Tên voucher";
            worksheet.Cells[1, 3].Value = "Mã voucher";
            worksheet.Cells[1, 4].Value = "Điểm đã đổi";
            worksheet.Cells[1, 5].Value = "Ngày hết hạn";
            worksheet.Cells[1, 6].Value = "Thời gian đổi";
            worksheet.Cells[1, 7].Value = "Trạng thái";

            for (int i = 0; i < viewModels.Count; i++)
            {
                var row = i + 2;
                var item = viewModels[i];
                worksheet.Cells[row, 1].Value = item.Redemption.UserVoucherId;
                worksheet.Cells[row, 2].Value = item.Voucher.Title;
                worksheet.Cells[row, 3].Value = item.Voucher.Code;
                worksheet.Cells[row, 4].Value = item.Voucher.PointsRequired;
                worksheet.Cells[row, 5].Value = item.Voucher.ExpirationDate?.ToString("dd/MM/yyyy");
                worksheet.Cells[row, 6].Value = item.Redemption.RedeemedAt?.ToString("dd/MM/yyyy HH:mm");
                worksheet.Cells[row, 7].Value = item.Redemption.IsUsed ? "Đã sử dụng" : "Chưa sử dụng";
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            string fileName = $"LichSuDoiVoucher_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
