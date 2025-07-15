using GreenCorner.MVC.Models;
using GreenCorner.MVC.Models.Notification;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace GreenCorner.MVC.Controllers
{
    public class TrashEventController : Controller
    {
        private readonly ITrashEventService _trashEventService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        public TrashEventController(ITrashEventService trashEventService, IUserService userService, INotificationService notificationService)
        {
            _trashEventService = trashEventService;
            _userService = userService;
            _notificationService = notificationService;
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
                    TempData["success"] = "Trash event reported successfully!";
                    return RedirectToAction(nameof(Index));
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
                TempData["success"] = "Trash Event deleted successfully!";
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
                TempData["success"] = "Trash event updated successfully!";
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
                TempData["error"] = "An error occurred while approving the trash event: " + ex.Message;
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
                TempData["error"] = "An error occurred while approving the trash event: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
