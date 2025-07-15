using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GreenCorner.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IVolunteerService _volunteerService;
        private readonly IEventService _eventService;
        private readonly ITrashEventService _trashEventService;
        private readonly IRewardRedemptionHistoryService _rewardRedemptionHistoryService;
        private readonly IVoucherService _voucherService;
        public UserController(IUserService userService, IVolunteerService volunteerService, IEventService eventService, ITrashEventService trashEventService, IRewardRedemptionHistoryService rewardRedemptionHistoryService, IVoucherService voucherService)
        {
            _userService = userService;
            _volunteerService = volunteerService;
            _eventService = eventService;
            _trashEventService = trashEventService;
            _voucherService = voucherService;
            _rewardRedemptionHistoryService = rewardRedemptionHistoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Bạn cần đăng nhập để xem hồ sơ.";
                return RedirectToAction("Login", "Auth");
            }

            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            var userViewModel = new UserDTO();
            var response = await _userService.GetUserById(userId);
            if (response != null && response.IsSuccess)
            {
                userViewModel = JsonConvert.DeserializeObject<UserDTO>(response.Result.ToString());
                return View(userViewModel);
            }
            else
            {
                TempData["error"] = response?.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdateProfile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Bạn cần đăng nhập để xem hồ sơ.";
                return RedirectToAction("Login", "Auth");
            }

            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            var userViewModel = new UserDTO();
            var response = await _userService.GetUserById(userId);
            if (response != null && response.IsSuccess)
            {
                userViewModel = JsonConvert.DeserializeObject<UserDTO>(response.Result.ToString());
                return View(userViewModel);
            }
            else
            {
                TempData["error"] = response.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(UserDTO user)
        {
            var files = Request.Form.Files;
            bool hasNewAvatar = files != null && files.Count > 0;

            if (hasNewAvatar)
            {
                if (!string.IsNullOrEmpty(user.Avatar))
                {
                    var oldAvatarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.Avatar.TrimStart('/'));
                    if (System.IO.File.Exists(oldAvatarPath))
                    {
                        System.IO.File.Delete(oldAvatarPath);
                    }
                }

                var (isSuccess, imagePaths, errorMessage) = await FileUploadHelper.UploadImagesStrictAsync(
                    files, folderName: "avatars", filePrefix: "avatar", onlyOneFile: true);

                if (!isSuccess)
                {
                    ModelState.AddModelError("Images", errorMessage);
                    return View(user);
                }

                user.Avatar = imagePaths.FirstOrDefault();
            }

            var response = await _userService.UpdateUser(user);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cập nhật hồ sơ thành công.";
                return RedirectToAction("Profile", "User");
            }
            else
            {
                TempData["error"] = response.Message;
                return RedirectToAction("UpdateProfile", "User");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestDTO changePasswordRequest)
        {
            if (ModelState.IsValid)
            {
                changePasswordRequest.UserID = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
                ResponseDTO response = await _userService.ChangePassword(changePasswordRequest);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Đổi mật khẩu thành công!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View();
        }

        public async Task<IActionResult> ViewParticipatedActivities()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Bạn cần đăng nhập để xem các hoạt động đã tham gia.";
                return RedirectToAction("Login", "Auth"); 
            }
            var response = await _volunteerService.GetParticipatedActivitiesByUserId(userId);
            List<VolunteerWithEventViewModel> list = new();

            if (response != null && response.IsSuccess)
            {
                var volunteers = JsonConvert.DeserializeObject<List<VolunteerDTO>>(response.Result.ToString());

                foreach (var v in volunteers)
                {
                    var eventResponse = await _eventService.GetByEventId(v.CleanEventId);
                    if (eventResponse != null && eventResponse.IsSuccess)
                    {
                        var evt = JsonConvert.DeserializeObject<EventDTO>(eventResponse.Result.ToString());
                        list.Add(new VolunteerWithEventViewModel
                        {
                            Volunteer = v,
                            Event = evt
                        });
                    }
                }
            }
            if (!list.Any())
            {
                TempData["error"] = "Bạn chưa tham gia hoạt động nào.";
                return RedirectToAction("Profile", "User"); 
            }
            var vm = new VolunteerEventListViewModel
            {
                Participations = list
            };

            return View(vm);
        }

        public async Task<IActionResult> ViewActivities(string userId)
        {
            //var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Bạn cần đăng nhập để xem các hoạt động đã tham gia.";
                return RedirectToAction("Login", "Auth");
            }
            var response = await _volunteerService.GetParticipatedActivitiesByUserId(userId);
            List<VolunteerWithEventViewModel> list = new();

            if (response != null && response.IsSuccess)
            {
                var volunteers = JsonConvert.DeserializeObject<List<VolunteerDTO>>(response.Result.ToString());

                foreach (var v in volunteers)
                {
                    var eventResponse = await _eventService.GetByEventId(v.CleanEventId);
                    if (eventResponse != null && eventResponse.IsSuccess)
                    {
                        var evt = JsonConvert.DeserializeObject<EventDTO>(eventResponse.Result.ToString());
                        list.Add(new VolunteerWithEventViewModel
                        {
                            Volunteer = v,
                            Event = evt
                        });
                    }
                }
            }

            var vm = new VolunteerEventListViewModel
            {
                Participations = list
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> ReportHistory()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Vui lòng đăng nhập để xem lịch sử báo cáo của bạn";
                return RedirectToAction("Index", "Home");
            }
            try
            {
                var userID = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
                var response = await _trashEventService.GetTrashEventsByUserId(userID);
                if (response != null && response.IsSuccess)
                {
                    List<TrashEventDTO> reportHistory = JsonConvert.DeserializeObject<List<TrashEventDTO>>(Convert.ToString(response.Result));
                    return View(reportHistory);
                }
                else
                {
                    TempData["error"] = response.Message;
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Achievements()
        {
            // Lấy userId từ claim
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Không tìm thấy người dùng.";
                return RedirectToAction("Login", "Auth");
            }

            // Gọi API lấy danh sách volunteer đã duyệt của user
            var volunteerRes = await _volunteerService.GetApprovedVolunteersByUserIdAsync(userId);
            if (volunteerRes == null || !volunteerRes.IsSuccess)
            {
                TempData["error"] = "Không thể tải dữ liệu tình nguyện viên.";
                return View(new VolunteerEventListViewModel { Participations = new List<VolunteerWithEventViewModel>() });
            }

            var volunteerList = JsonConvert.DeserializeObject<List<VolunteerDTO>>(volunteerRes.Result.ToString()!);
            var eventIds = volunteerList.Select(v => v.CleanEventId).Distinct().ToList();

            // Gọi API lấy danh sách sự kiện theo danh sách ID
            var eventRes = await _eventService.GetEventsByIdsAsync(eventIds);
            if (eventRes == null || !eventRes.IsSuccess)
            {
                TempData["error"] = "Không thể tải dữ liệu sự kiện.";
                return View(new VolunteerEventListViewModel { Participations = new List<VolunteerWithEventViewModel>() });
            }

            var eventList = JsonConvert.DeserializeObject<List<EventDTO>>(eventRes.Result.ToString()!);

            // Kết hợp dữ liệu thành ViewModel
            var participations = (from v in volunteerList
                                  join e in eventList on v.CleanEventId equals e.CleanEventId
                                  select new VolunteerWithEventViewModel
                                  {
                                      Volunteer = v,
                                      Event = e
                                  }).ToList();

            var viewModel = new VolunteerEventListViewModel
            {
                Participations = participations
            };

            return View(viewModel);
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
