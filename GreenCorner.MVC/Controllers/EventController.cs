using GreenCorner.MVC.Models;
using GreenCorner.MVC.Models.Notification;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GreenCorner.MVC.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IVolunteerService _volunteerService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        public EventController(IEventService eventService, INotificationService notificationService, IVolunteerService volunteerService, IUserService userService)
        {
            _eventService = eventService;
            _volunteerService = volunteerService;
            _userService = userService;
            _notificationService = notificationService;
        }
        public async Task<IActionResult> Index()
        {
            List<EventDTO> listEvent = new();
            ResponseDTO? response = await _eventService.GetOpenEvent();
            if (response != null && response.IsSuccess)
            {
                listEvent = JsonConvert.DeserializeObject<List<EventDTO>>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(listEvent);
        }
        public async Task<IActionResult> GetAllEvent()
        {
            List<GetAllEventViewModel> viewModelList = new();
            List<EventDTO> listEvent = new();
            ResponseDTO? response = await _eventService.GetAllEvent();
            if (response != null && response.IsSuccess)
            {
                listEvent = JsonConvert.DeserializeObject<List<EventDTO>>(response.Result.ToString());
                foreach (var events in listEvent)
                {
                    ResponseDTO? eventResponse = await _eventService.GetParticipationInfoAsync(events.CleanEventId);

                    ParticipationInfoResponse participation = new();
                    if (eventResponse != null && eventResponse.IsSuccess)
                    {
                        participation = JsonConvert.DeserializeObject<ParticipationInfoResponse>(eventResponse.Result.ToString());
                    }

                    viewModelList.Add(new GetAllEventViewModel
                    {
                        Event = events,
                        Participation = participation
                    });
                }
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(viewModelList);
        }
        public async Task<IActionResult> GetEventById(int eventId)
        {
            ResponseDTO response = await _eventService.GetByEventId(eventId);
            if (response != null && response.IsSuccess)
            {
                EventDTO eventDTO = JsonConvert.DeserializeObject<EventDTO>(response.Result.ToString());

                var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;

                bool isVolunteer = false;
                bool isTeamLeader = false;
                string? approvedRole = null;
                bool hasApprovedTeamLeader = false;

                if (!string.IsNullOrEmpty(userId))
                {
                    // Gọi các API kiểm tra trạng thái
                    var volunteerResponse = await _volunteerService.IsVolunteer(eventId, userId);
                    var teamLeaderResponse = await _volunteerService.IsTeamLeader(eventId, userId);
                    var roleResponse = await _volunteerService.GetApprovedRoleAsync(eventId, userId);
                    var hasTLResponse = await _volunteerService.HasApprovedTeamLeaderAsync(eventId);

                    if (volunteerResponse?.IsSuccess == true && volunteerResponse.Result is bool v)
                        isVolunteer = v;

                    if (teamLeaderResponse?.IsSuccess == true && teamLeaderResponse.Result is bool t)
                        isTeamLeader = t;

                    if (roleResponse?.IsSuccess == true)
                        approvedRole = roleResponse.Result?.ToString();

                    if (hasTLResponse?.IsSuccess == true && hasTLResponse.Result is bool h)
                        hasApprovedTeamLeader = h;
                }

                // Gọi API lấy thông tin tham gia
                var participationResponse = await _eventService.GetParticipationInfoAsync(eventId);
                int current = 0, max = 0;
                if (participationResponse?.IsSuccess == true && participationResponse.Result != null)
                {
                    var json = JsonConvert.SerializeObject(participationResponse.Result);
                    var info = JsonConvert.DeserializeObject<ParticipationInfoResponse>(json);
                    current = info!.Current;
                    max = info.Max;
                }

                // Gọi API kiểm tra đầy
                var isFullResponse = await _eventService.CheckEventIsFullAsync(eventId);
                bool isFull = false;
                if (isFullResponse?.IsSuccess == true && isFullResponse.Result != null)
                {
                    var json = JsonConvert.SerializeObject(isFullResponse.Result);
                    var info = JsonConvert.DeserializeObject<IsFullResponse>(json);
                    isFull = info!.IsFull;
                }

                // Truyền ViewBag cho view
                ViewBag.IsVolunteer = isVolunteer;
                ViewBag.IsTeamLeader = isTeamLeader;
                ViewBag.ApprovedRole = approvedRole;
                ViewBag.HasApprovedTeamLeader = hasApprovedTeamLeader;

                ViewBag.ParticipantCurrent = current;
                ViewBag.ParticipantMax = max;
                ViewBag.IsFull = isFull;


                return View(eventDTO);
            }

            TempData["error"] = response?.Message;
            return NotFound();
        }

        public async Task<IActionResult> RateEvent(int eventId)
        {
            ViewBag.EventId = eventId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RateEvent(EventReviewDTO eventReviewDTO)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Bạn cần đăng nhập để xem hồ sơ của bạn.";
                return RedirectToAction("Login", "Auth");
            }
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            eventReviewDTO.UserId = userId;
            eventReviewDTO.CreatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                ResponseDTO response = await _eventService.RateEvent(eventReviewDTO);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Đánh giá sự kiện thành công!";
                    return RedirectToAction("ViewParticipatedActivities", "User");
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(eventReviewDTO);
        }
        public async Task<IActionResult> EventReviewHistory()
        {
            List<EventReviewDTO> listEventReview = new();
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Bạn cần đăng nhập để xem hồ sơ của bạn.";
                return RedirectToAction("Login", "Auth");
            }

            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            ResponseDTO? response = await _eventService.ViewEventReviewHistory(userId);
            if (response != null && response.IsSuccess)
            {
                listEventReview = JsonConvert.DeserializeObject<List<EventReviewDTO>>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(listEventReview);
        }

        public async Task<IActionResult> DeleteEventReview(int eventReviewId)
        {
            ResponseDTO response = await _eventService.GetEventReviewById(eventReviewId);
            if (response != null && response.IsSuccess)
            {
                EventReviewDTO eventReviewDTO = JsonConvert.DeserializeObject<EventReviewDTO>(response.Result.ToString());
                return View(eventReviewDTO);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEventReview(EventReviewDTO eventReviewDTO)
        {
            ResponseDTO response = await _eventService.DeleteEventReview(eventReviewDTO.EventReviewId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Xóa đánh giá sự kiện thành công!";
                return RedirectToAction(nameof(EventReviewHistory));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(eventReviewDTO);
        }

        public async Task<IActionResult> EditEventReview(int eventReviewId)
        {
            ResponseDTO response = await _eventService.GetEventReviewById(eventReviewId);
            if (response != null && response.IsSuccess)
            {
                EventReviewDTO eventReview = JsonConvert.DeserializeObject<EventReviewDTO>(response.Result.ToString());
                return View(eventReview);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditEventReview(EventReviewDTO eventReviewDTO)
        {
            ResponseDTO response = await _eventService.EditEventReview(eventReviewDTO);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Đánh giá sự kiện đã được cập nhật thành công!";
                return RedirectToAction(nameof(EventReviewHistory));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(eventReviewDTO);
        }
        public async Task<IActionResult> LeaderReview(int eventId, string userId)
        {
            ViewBag.EventId = eventId;
            ViewBag.UserId = userId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LeaderReview(LeaderReviewDTO leaderReviewDTO)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Bạn cần đăng nhập để xem hồ sơ của bạn.";
                return RedirectToAction("Login", "Auth");
            }

            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            leaderReviewDTO.CreatedAt = DateTime.Now;
            leaderReviewDTO.ReviewerId = userId;
            if (ModelState.IsValid)
            {
                ResponseDTO response = await _eventService.LeaderReview(leaderReviewDTO);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Đánh giá đội trưởng thành công!";
                    return RedirectToAction("ViewParticipatedActivities", "User");
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(leaderReviewDTO);
        }
        public async Task<IActionResult> LeaderReviewHistory()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Bạn cần đăng nhập để xem hồ sơ của bạn.";
                return RedirectToAction("Login", "Auth");
            }

            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            List<LeaderReviewDTO> listLeaderReview = new();
            ResponseDTO? response = await _eventService.ViewLeaderReviewHistory(userId);

            if (response != null && response.IsSuccess)
            {
                listLeaderReview = JsonConvert.DeserializeObject<List<LeaderReviewDTO>>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(listLeaderReview);
        }

        public async Task<IActionResult> DeleteLeaderReview(int leaderReviewId)
        {
            ResponseDTO response = await _eventService.GetLeaderReviewById(leaderReviewId);
            if (response != null && response.IsSuccess)
            {
                LeaderReviewDTO leaderReviewDTO = JsonConvert.DeserializeObject<LeaderReviewDTO>(response.Result.ToString());
                return View(leaderReviewDTO);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLeaderReview(LeaderReviewDTO leaderReviewDTO)
        {
            ResponseDTO response = await _eventService.DeleteLeaderReview(leaderReviewDTO.LeaderReviewId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Xóa đánh giá đội trưởng thành công!";
                return RedirectToAction(nameof(LeaderReviewHistory));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(leaderReviewDTO);
        }

        public async Task<IActionResult> EditLeaderReview(int leaderReviewId)
        {
            ResponseDTO response = await _eventService.GetLeaderReviewById(leaderReviewId);
            if (response != null && response.IsSuccess)
            {
                LeaderReviewDTO leaderReviewDTO = JsonConvert.DeserializeObject<LeaderReviewDTO>(response.Result.ToString());
                return View(leaderReviewDTO);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditLeaderReview(LeaderReviewDTO leaderReviewDTO)
        {
            ResponseDTO response = await _eventService.EditLeaderReview(leaderReviewDTO);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cập nhật đánh giá đội trưởng thành công!";
                return RedirectToAction(nameof(LeaderReviewHistory));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(leaderReviewDTO);
        }

        public async Task<IActionResult> ViewEventVolunteerListCheck(int eventId)
        {
            List<EventVolunteerWithUserViewModel> volunteerWithUsers = new();

            ResponseDTO? response = await _eventService.ViewEventVolunteerList(eventId);
            if (response != null && response.IsSuccess && response.Result != null)
            {
                var eventVolunteers = JsonConvert.DeserializeObject<List<EventVolunteerDTO>>(response.Result.ToString());

                foreach (var ev in eventVolunteers)
                {
                    UserDTO? user = null;

                    var userResponse = await _userService.GetUserById(ev.UserId);
                    if (userResponse != null && userResponse.IsSuccess && userResponse.Result != null)
                    {
                        user = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result.ToString());
                    }

                    volunteerWithUsers.Add(new EventVolunteerWithUserViewModel
                    {
                        Volunteer = ev,
                        User = user
                    });
                }
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(volunteerWithUsers);
        }

        [HttpGet]
        public async Task<IActionResult> AttendanceCheck(string userId,int eventId, bool check)
        {

			ResponseDTO response = await _eventService.AttendanceCheck(userId, eventId, check);
			if (response != null && response.IsSuccess)
			{
                return RedirectToAction(nameof(ViewEventVolunteerListCheck),new { eventId = eventId });
			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return NotFound();
		}
		public async Task<IActionResult> EditAttendance(string userId, int eventId)
		{

			ResponseDTO response = await _eventService.EditAttendance(userId, eventId);
			if (response != null && response.IsSuccess)
			{
                TempData["success"] = "Điểm danh lại thành công.";
                return RedirectToAction(nameof(ViewEventVolunteerListCheck), new { eventId = eventId });
			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return NotFound();
		}


		//Dangky Volunteer
		[HttpGet]
        public IActionResult RegisterVolunteer(int eventId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Vui lòng đăng nhập để tiếp tục.";
                return RedirectToAction("Login", "Auth");
            }
            var dto = new VolunteerDTO
            {
                CleanEventId = eventId,
                UserId = userId,
                ApplicationType = "Volunteer"
            };

            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> RegisterVolunteer(VolunteerDTO dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Form không hợp lệ.";
                return View(dto);
            }
            var response = await _volunteerService.RegisterVolunteer(dto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response.Message;
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return RedirectToAction("GetEventById", "Event", new { eventId = dto.CleanEventId });
        }

        //Dangky TeamLeader
        [HttpGet]
        public IActionResult RegisterTeamLeader(int eventId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Vui lòng đăng nhập để tiếp tục.";
                return RedirectToAction("Login", "Auth");
            }
            var dto = new VolunteerDTO
            {
                CleanEventId = eventId,
                UserId = userId,
                ApplicationType = "TeamLeader"
            };

            return View(dto);
        }


        [HttpPost]
        public async Task<IActionResult> RegisterTeamLeader(VolunteerDTO dto)
        {
            var response = await _volunteerService.RegisterVolunteer(dto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response.Message;
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return RedirectToAction("GetEventById", "Event", new { eventId = dto.CleanEventId });
        }

        //Huydangky
        [HttpPost]
        public async Task<IActionResult> UnregisterVolunteer(int eventId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            string role = "Volunteer";
            var response = await _volunteerService.UnregisterAsync(eventId, userId, role);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response.Message;
            }
            else
            {
                TempData["error"] = response?.Message ?? "Có lỗi xảy ra.";
            }

            return RedirectToAction(nameof(GetEventById), new { eventId });
        }

        [HttpPost]
        public async Task<IActionResult> UnregisterTeamLeader(int eventId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            string role = "TeamLeader";
            var response = await _volunteerService.UnregisterAsync(eventId, userId, role);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response.Message;
            }
            else
            {
                TempData["error"] = response?.Message ?? "Có lỗi xảy ra.";
            }

            return RedirectToAction(nameof(GetEventById), new { eventId });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRegister(int eventId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Kiểm xem người dùng đã đăng ký chưa
            var volunteerResponse = await _volunteerService.IsVolunteer(eventId, userId);
            var teamLeaderResponse = await _volunteerService.IsTeamLeader(eventId, userId);

            bool isVolunteer = volunteerResponse?.IsSuccess == true &&
                               volunteerResponse?.Result is bool v && v;

            bool isTeamLeader = teamLeaderResponse?.IsSuccess == true &&
                                teamLeaderResponse?.Result is bool t && t;

            var dto = new VolunteerDTO
            {
                CleanEventId = eventId,
                UserId = userId,
            };

            if (isVolunteer)
            {
                dto.ApplicationType = "Volunteer";
            }
            else if (isTeamLeader)
            {
                dto.ApplicationType = "TeamLeader";
            }           
            return View(dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRegister(VolunteerDTO volunteerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(volunteerDto);
            }
            try
            {
                await _volunteerService.UpdateRegister(volunteerDto);
                TempData["Success"] = "Cập nhật đăng ký thành công.";
                return RedirectToAction("GetEventById", "Event", new { eventId = volunteerDto.CleanEventId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(volunteerDto);
            }
        }
        public async Task<IActionResult> CreateCleanupEvent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCleanupEvent(EventDTO eventDTO)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Bạn cần đăng nhập để xem hồ sơ của bạn.";
                return RedirectToAction("Login", "Auth");
            }
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            eventDTO.CreatedAt = DateTime.Now;
            eventDTO.Status = "Open";
            var files = Request.Form.Files;
            var (isSuccess, imagePaths, errorMessage) = await FileUploadHelper.UploadImagesStrictAsync(
                files, folderName: "event", filePrefix: "event");
           
            if (!isSuccess)
            {
                ModelState.AddModelError("Image", errorMessage);
                return View(eventDTO);
            }

            eventDTO.ImageUrl = imagePaths.FirstOrDefault();

            if (ModelState.IsValid)
            {
                ResponseDTO response = await _eventService.CreateCleanupEvent(eventDTO);
                if (response != null && response.IsSuccess)
                {
                    var listUser = await _userService.GetActiveUser();
                    if (listUser != null && listUser.IsSuccess)
                    {
                        List<UserDTO> users = listUser.Result != null ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserDTO>>(listUser.Result.ToString()) : new List<UserDTO>();
                        foreach (UserDTO user in users)
                        {
                            var notification = new NotificationDTO
                            {
                                UserId = user.ID,
                                Title = "Sự kiện dòn rác vừa được tạo",
                                Message = $"Hãy tham gia và chia sẻ sự kiện này để thu hút thêm tình nguyện viên cùng tham gia bảo vệ môi trường!."
                            };
                            var sendNotification = await _notificationService.SendNotification(notification);
                        }
                    }
                    TempData["success"] = "Tạo sự kiện dọn dẹp thành công!";
                    return RedirectToAction(nameof(GetAllEvent));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(eventDTO);
        }
        public async Task<IActionResult> UpdateCleanupEvent(int eventId)
        {
            ResponseDTO response = await _eventService.GetByEventId(eventId);
            if (response != null && response.IsSuccess)
            {
                EventDTO eventDTO = JsonConvert.DeserializeObject<EventDTO>(response.Result.ToString());
                return View(eventDTO);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCleanupEvent(EventDTO eventDTO)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Bạn cần đăng nhập để thực hiện chức năng này";
                return RedirectToAction("Login", "Auth");
            }

            var files = Request.Form.Files;
            bool hasNewImages = files != null && files.Count > 0;

            if (hasNewImages)
            {
                // Xoá ảnh cũ nếu có
                if (!string.IsNullOrEmpty(eventDTO.ImageUrl))
                {
                    foreach (var oldPath in eventDTO.ImageUrl.Split("&"))
                    {
                        var fullOldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldPath.TrimStart('/'));
                        if (System.IO.File.Exists(fullOldPath))
                        {
                            System.IO.File.Delete(fullOldPath);
                        }
                    }
                }

                // Upload ảnh mới
                var (isSuccess, imagePaths, errorMessage) = await FileUploadHelper.UploadImagesStrictAsync(
                    files, folderName: "event", filePrefix: "event");

                if (!isSuccess)
                {
                    ModelState.AddModelError("Image", errorMessage);
                    return View(eventDTO);
                }

                eventDTO.ImageUrl = string.Join("&", imagePaths);
            }
            ResponseDTO response = await _eventService.UpdateCleanupEvent(eventDTO);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cập nhật sự kiện dọn dẹp thành công!";
                return RedirectToAction(nameof(GetAllEvent));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(eventDTO);
        }
        public async Task<IActionResult> UpdateCleanupEventStatus(int eventId)
        {
            ResponseDTO response = await _eventService.GetByEventId(eventId);
            if (response != null && response.IsSuccess)
            {
                EventDTO eventDTO = JsonConvert.DeserializeObject<EventDTO>(response.Result.ToString());
                return View(eventDTO);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCleanupEventStatus(EventDTO eventDTO)
        {
            ResponseDTO response = await _eventService.UpdateCleanupEventStatus(eventDTO);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cập nhật trạng thái sự kiện dọn dẹp thành công!";
                return RedirectToAction(nameof(GetAllEvent));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(eventDTO);
        }
        public async Task<IActionResult> CloseCleanupEvent(int eventId)
        {
			try
			{
                
                var responseDate = await _eventService.GetByEventId(eventId);
                EventDTO eventDTO = JsonConvert.DeserializeObject<EventDTO>(responseDate.Result.ToString());
                if (DateTime.Now < eventDTO.EndDate)
                {
                    TempData["success"] = "Chưa đến thời gian kết thúc sự kiện!";
                }
                else
                {
                    var responseDeleteCleanEvent = await _eventService.DeleteVolunteersByEventId(eventId);
                    if(responseDeleteCleanEvent!=null && responseDeleteCleanEvent.IsSuccess)
                    {
                        var updateVolunteerStatus = await _eventService.UpdateVolunteerStatusToParticipated(eventId);
                        if(updateVolunteerStatus != null && updateVolunteerStatus.IsSuccess)
                        {
                            var response = await _eventService.CloseCleanupEvent(eventId);
                            if (response != null && response.IsSuccess)
                            {
                                TempData["success"] = "Kết thúc sự kiện dọn dẹp thành công!";
                            }
                            else
                            {
                                TempData["error"] = response?.Message;
                            }
                        }
                        else
                        {
                            TempData["error"] = responseDeleteCleanEvent?.Message;
                        }
                    }
                    else
                    {
                        TempData["error"] = responseDeleteCleanEvent?.Message;
                    }

                }
				return RedirectToAction("GetAllEvent", "Event");
			}
			catch (Exception ex)
			{
				TempData["error"] = ex.Message;
				return RedirectToAction("GetAllEvent", "Event");
			}
		}
		public async Task<IActionResult> OpenCleanupEvent(int eventId)
		{
			try
			{
				var response = await _eventService.OpenCleanupEvent(eventId);
				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Bắt đầu sự kiện thành công";
				}
				else
				{
					TempData["error"] = response?.Message;
				}
				return RedirectToAction("GetAllEvent", "Event");
			}
			catch (Exception ex)
			{
				TempData["error"] = ex.Message;
				return RedirectToAction("GetAllEvent", "Event");
			}
		}
        public async Task<IActionResult> ViewEventVolunteerList(int eventId)
        {
            List<EventVolunteerWithUserViewModel> viewModelList = new();

            ResponseDTO? response = await _eventService.ViewEventVolunteerList(eventId);

            if (response != null && response.IsSuccess && response.Result != null)
            {
                var volunteerList = JsonConvert.DeserializeObject<List<EventVolunteerDTO>>(response.Result.ToString());

                foreach (var item in volunteerList)
                {
                    UserDTO? user = null;

                    var userResponse = await _userService.GetUserById(item.UserId);
                    if (userResponse != null && userResponse.IsSuccess && userResponse.Result != null)
                    {
                        user = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result.ToString());
                    }

                    viewModelList.Add(new EventVolunteerWithUserViewModel
                    {
                        Volunteer = item,
                        User = user!
                    });
                }
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(viewModelList);
        }


        public async Task<IActionResult> KickVolunteer(string userId, int eventId)
        {

            ResponseDTO response = await _eventService.KickVolunteer(userId, eventId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Đã xóa thành viên khỏi danh sách tham gia.";
                return RedirectToAction(nameof(ViewEventVolunteerList), new { eventId = eventId });
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }
        public async Task<IActionResult> GetOpenEventsByTeamLeader()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Vui lòng đăng nhập để xem các sự kiện do bạn quản lý!";
                return RedirectToAction("Login", "Auth");
            }
                var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
                List<EventDTO> listEvent = new();
            ResponseDTO? response = await _eventService.GetOpenEventsByTeamLeader(userId);
            if (response != null && response.IsSuccess)
            {
                listEvent = JsonConvert.DeserializeObject<List<EventDTO>>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(listEvent);
        }

        [HttpGet]
        public async Task<IActionResult> HasApprovedTeamLeader(int eventId)
        {
            var response = await _volunteerService.HasApprovedTeamLeaderAsync(eventId);
            if (response != null && response.IsSuccess)
            {
                ViewBag.HasApprovedTeamLeader = response.Result;
            }
            else
            {
                TempData["error"] = response?.Message ?? "Lỗi khi kiểm tra TeamLeader đã được duyệt.";
                ViewBag.HasApprovedTeamLeader = false;
            }

            return RedirectToAction(nameof(GetEventById), new { eventId });
        }

        [HttpGet]
        public async Task<IActionResult> GetApprovedRole(int eventId)
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["loginError"] = "Bạn cần đăng nhập để thực hiện chức năng này.";
                return RedirectToAction("Login", "Auth");
            }

            var response = await _volunteerService.GetApprovedRoleAsync(eventId, userId);
            if (response != null && response.IsSuccess)
            {
                ViewBag.ApprovedRole = response.Result?.ToString();
            }
            else
            {
                TempData["error"] = response?.Message ?? "Không thể kiểm tra vai trò đã được duyệt.";
            }

            return RedirectToAction(nameof(GetEventById), new { eventId });
        }
    }
}
