using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using GreenCorner.MVC.Models;
using GreenCorner.MVC.Models.Notification;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;

namespace GreenCorner.MVC.Controllers
{
    public class TrashEventController : Controller
    {
        private readonly ITrashEventService _trashEventService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IAdminService _adminService;
        private readonly IPointTransactionService _pointTransactionService;
        public TrashEventController(ITrashEventService trashEventService, IUserService userService, INotificationService notificationService, IAdminService adminService, IPointTransactionService pointTransactionService)
        {
            _trashEventService = trashEventService;
            _userService = userService;
            _notificationService = notificationService;
            _adminService = adminService;
            _pointTransactionService = pointTransactionService;
        }

        public async Task<IActionResult> Index()
        {
            List<TrashReportListViewModel> viewModelList = new();
            ResponseDTO? response = await _trashEventService.GetAllTrashEvent();
            if (response != null && response.IsSuccess)
            {
                var trashEvents = JsonConvert.DeserializeObject<List<TrashEventDTO>>(response.Result.ToString());
                foreach (var trashEvent in trashEvents)
                {
                    ResponseDTO? userResponse = await _userService.GetUserById(trashEvent.UserId);

                    UserDTO userDTO = new UserDTO();

                    if (userResponse != null && userResponse.IsSuccess)
                    {
                        userDTO = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result.ToString());
                    }

                    viewModelList.Add(new TrashReportListViewModel
                    {
                        TrashEvent = trashEvent,
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

        [HttpGet]
        public async Task<IActionResult> Detail(int trashReportId)
        {


            ResponseDTO response = await _trashEventService.GetByTrashEventId(trashReportId);
            if (response != null && response.IsSuccess)
            {
                TrashEventDTO trashEventDTO = JsonConvert.DeserializeObject<TrashEventDTO>(response.Result.ToString());
                ResponseDTO? userResponse = await _userService.GetUserById(trashEventDTO.UserId);

                UserDTO userDTO = new UserDTO();

                if (userResponse != null && userResponse.IsSuccess)
                {
                    userDTO = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result.ToString());
                }
                TrashReportListViewModel viewModel = new()
                {
                    TrashEvent = trashEventDTO,
                    User = userDTO
                };

                return View(viewModel);
            }
            else
            {
                TempData["error"] = response?.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> ReportTrashEvent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReportTrashEvent(TrashEventDTO trashEventDTO)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Bạn cần đăng nhập để thực hiện được chức năng này";
                return RedirectToAction("Login", "Auth");
            }

            var files = Request.Form.Files;

            var (isSuccess, imagePaths, errorMessage) = await FileUploadHelper.UploadImagesStrictAsync(
                files, folderName: "reporttrash", filePrefix: "reporttrash");

            if (!isSuccess)
            {
                ModelState.AddModelError("Images", errorMessage);
                return View(trashEventDTO);
            }

            trashEventDTO.ImageUrl = string.Join("&", imagePaths);

            if (!ModelState.IsValid)
            {
                trashEventDTO.UserId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
                trashEventDTO.CreatedAt = DateTime.Now;
                trashEventDTO.Status = "Chờ xác nhận";
               

                ResponseDTO response = await _trashEventService.AddTrashEvent(trashEventDTO);
                if (response != null && response.IsSuccess)
                {
                    var pattern = @"\b(Phường|Xã)\s+[^,]+";
                    var match = Regex.Match(trashEventDTO.Address, pattern, RegexOptions.IgnoreCase);
                    var listUser = await _userService.GetNearUser(match.Value);
                    if (listUser != null && listUser.IsSuccess)
                    {
                        List<UserDTO> users = listUser.Result != null ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserDTO>>(listUser.Result.ToString()) : new List<UserDTO>();
                        foreach (UserDTO user in users)
                        {
                            var notification = new NotificationDTO
                            {
                                UserId = user.ID,
                                Title = "Có báo cáo rác gần nơi bạn sinh sống",
                                Message = $"Có báo cáo điểm rác ở '{trashEventDTO.Address}' hãy kiểm tra xác nhận hộ chúng mình nhé!."
                            };
                            var sendNotification = await _notificationService.SendNotification(notification);
                        }
                    }
                    TempData["success"] = "Sự kiện rác đã được báo cáo thành công!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(trashEventDTO);
        }


        public async Task<IActionResult> DeleteTrashEvent(int trashReportId)
        {
            ResponseDTO response = await _trashEventService.GetByTrashEventId(trashReportId);
            if (response != null && response.IsSuccess)
            {
                TrashEventDTO trashEventDTO = JsonConvert.DeserializeObject<TrashEventDTO>(response.Result.ToString());
                return View(trashEventDTO);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTrashEvent(TrashEventDTO trashEventDTO)
        {
            ResponseDTO response = await _trashEventService.DeleteTrashEvent(trashEventDTO.TrashReportId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Sự kiện Thùng rác đã được xóa thành công!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(trashEventDTO);
        }

        public async Task<IActionResult> UpdateTrashEvent(int trashReportId)
        {
            ResponseDTO response = await _trashEventService.GetByTrashEventId(trashReportId);
            if (response != null && response.IsSuccess)
            {
                TrashEventDTO trashEventDTO = JsonConvert.DeserializeObject<TrashEventDTO>(response.Result.ToString());
                return View(trashEventDTO);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTrashEvent(TrashEventDTO trashEventDTO)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Bạn cần đăng nhập để thực hiện được chức năng này";
                return RedirectToAction("Login", "Auth");
            }

            var files = Request.Form.Files;

            bool hasNewImages = files != null && files.Count > 0;

            if (hasNewImages)
            {
                if (!string.IsNullOrEmpty(trashEventDTO.ImageUrl))
                {
                    foreach (var oldPath in trashEventDTO.ImageUrl.Split("&"))
                    {
                        var fullOldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldPath.TrimStart('/'));
                        if (System.IO.File.Exists(fullOldPath))
                        {
                            System.IO.File.Delete(fullOldPath);
                        }
                    }
                }

                var (isSuccess, imagePaths, errorMessage) = await FileUploadHelper.UploadImagesStrictAsync(
                    files, folderName: "reporttrash", filePrefix: "reporttrash");

                if (!isSuccess)
                {
                    ModelState.AddModelError("Images", errorMessage);
                    return View(trashEventDTO);
                }

                trashEventDTO.ImageUrl = string.Join("&", imagePaths);
            }
            trashEventDTO.UserId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            ResponseDTO response = await _trashEventService.UpdateTrashEvent(trashEventDTO);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Sự kiện rác đã được cập nhật thành công!";
                return RedirectToAction("ReportHistory", "User");
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(trashEventDTO);
        }

        [HttpGet]
        public async Task<IActionResult> ApproveTrashEvent(int trashReportId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Bạn cần đăng nhập để thực hiện được chức năng này";
                return RedirectToAction("Login", "Auth");
            }
            try
            {
                ResponseDTO response = await _trashEventService.ApproveTrashEvent(trashReportId);
                if (response != null && response.IsSuccess)
                {
                    ResponseDTO responseTrash = await _trashEventService.GetByTrashEventId(trashReportId);
                    if (responseTrash != null && responseTrash.IsSuccess)
                    {
                        TrashEventDTO trashEventDto = JsonConvert.DeserializeObject<TrashEventDTO>(responseTrash.Result.ToString());
                        var notification = new NotificationDTO
                        {
                            UserId = trashEventDto.UserId,
                            Title = "Báo cáo rác được xác nhận",
                            Message = $"Báo cáo của bạn tại {trashEventDto.Address} đã được xác nhận."
                        };
                        var sendNotification = await _notificationService.SendNotification(notification);
                        if (sendNotification != null && sendNotification.IsSuccess)
                        {
                            TempData["success"] = "Đã xác nhận sự kiện và gửi thông báo cho người dùng!";
                            var StaffName = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Name).FirstOrDefault()?.Value;
                            var log = new SystemLogDTO()
                            {
                                UserId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value,
                                ActionType = "Xác nhận báo cáo",
                                ObjectType = "Sự kiện",
                                ObjectId = trashReportId,
                                Description = $"Nhân viên {StaffName} đã xác nhận báo cáo của sự kiện với ID {trashReportId}.",
                                CreatedAt = DateTime.Now,
                            };
                            var logResponse = await _adminService.AddLogStaff(log);
                            var pointDto = new PointTransactionDTO
                            {
                                UserId = trashEventDto.UserId,
                                Points = 50, 
                                Type = "Thưởng" 
                            };

                            var rewardResponse = await _pointTransactionService.TransactionPoints(pointDto);
                            if (rewardResponse != null && rewardResponse.IsSuccess)
                            {
                                TempData["success"] += $" Bạn đã thưởng {pointDto.Points} điểm cho người dùng.";
                            }
                            else
                            {
                                TempData["error"] = "Xác nhận thành công, nhưng thưởng điểm thất bại.";
                            }
                        }
                        else
                        {
                            TempData["error"] = sendNotification?.Message ?? "Đã có lỗi xảy ra.";
                        }
                    }
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["error"] = "Đã xảy ra lỗi khi chấp thuận sự kiện rác: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> RejectrashEvent(int trashReportId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Bạn cần đăng nhập để thực hiện được chức năng này";
                return RedirectToAction("Login", "Auth");
            }
            try
            {
                ResponseDTO response = await _trashEventService.RejectTrashEvent(trashReportId);
                if (response != null && response.IsSuccess)
                {
                    ResponseDTO responseTrash = await _trashEventService.GetByTrashEventId(trashReportId);
                    if (responseTrash != null && responseTrash.IsSuccess)
                    {
                        TrashEventDTO trashEventDto = JsonConvert.DeserializeObject<TrashEventDTO>(responseTrash.Result.ToString());
                        var notification = new NotificationDTO
                        {
                            UserId = trashEventDto.UserId,
                            Title = "Báo cáo rác bị từ chối",
                            Message = $"Báo cáo của bạn tại {trashEventDto.Address} đã bị từ chối."
                        };
                        var sendNotification = await _notificationService.SendNotification(notification);
                        if (sendNotification != null && sendNotification.IsSuccess)
                        {
                            TempData["success"] = "Đã từ chối sự kiện và gửi thông báo cho người dùng!";
                            var StaffName = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Name).FirstOrDefault()?.Value;
                            var log = new SystemLogDTO()
                            {
                                UserId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value,
                                ActionType = "Từ chối báo cáo",
                                ObjectType = "Sự kiện",
                                ObjectId = trashReportId,
                                Description = $"Nhân viên {StaffName} đã từ chối báo cáo của sự kiện với ID {trashReportId}.",
                                CreatedAt = DateTime.Now,
                            };
                            var logResponse = await _adminService.AddLogStaff(log);
                        }
                        else
                        {
                            TempData["error"] = sendNotification?.Message ?? "Đã có lỗi xảy ra.";
                        }
                    }
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["error"] = "Đã xảy ra lỗi khi từ chối sự kiện rác: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
