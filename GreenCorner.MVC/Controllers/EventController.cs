using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GreenCorner.MVC.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IVolunteerService _volunteerService;
        public EventController(IEventService eventService, IVolunteerService volunteerService)
        {
            _eventService = eventService;
            _volunteerService = volunteerService;
        }
        public async Task<IActionResult> Index()
        {
            List<EventDTO> listEvent = new();
            ResponseDTO? response = await _eventService.GetAllEvent();
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
        public async Task<IActionResult> GetEventById(int eventId)
        {
            
            ResponseDTO response = await _eventService.GetByEventId(eventId);
                if (response != null && response.IsSuccess)
                {
                    EventDTO eventDTO = JsonConvert.DeserializeObject<EventDTO>(response.Result.ToString());
                var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;

                bool isVolunteer = false;
                bool isTeamLeader = false;

                if (!string.IsNullOrEmpty(userId))
                {
                    // Gọi API kiểm tra vai trò
                    var volunteerResponse = await _volunteerService.IsVolunteer(eventId, userId);
                    var teamLeaderResponse = await _volunteerService.IsTeamLeader(eventId, userId);

                    if (volunteerResponse?.IsSuccess == true && volunteerResponse.Result is bool v)
                        isVolunteer = v;

                    if (teamLeaderResponse?.IsSuccess == true && teamLeaderResponse.Result is bool t)
                        isTeamLeader = t;
                }

                // Truyền xuống View
                ViewBag.IsVolunteer = isVolunteer;
                ViewBag.IsTeamLeader = isTeamLeader;
                return View(eventDTO);
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                return NotFound();
            
        }
        public async Task<IActionResult> RateEvent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RateEvent(EventReviewDTO eventReviewDTO)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "You need to log in to view your profile.";
                return RedirectToAction("Login", "Auth");
            }

            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            eventReviewDTO.CleanEventId = 2;
            eventReviewDTO.UserId = userId;
            eventReviewDTO.CreatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                ResponseDTO response = await _eventService.RateEvent(eventReviewDTO);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Event Rate successfully!";
                    return RedirectToAction(nameof(EventReviewHistory));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(eventReviewDTO);
        }
        public async Task<IActionResult> EventReviewHistory(string id)
        {
            List<EventReviewDTO> listEventReview = new();
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "You need to log in to view your profile.";
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
                TempData["success"] = "Event review deleted successfully!";
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
                TempData["success"] = "Event review updated successfully!";
                return RedirectToAction(nameof(EventReviewHistory));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(eventReviewDTO);
        }
        public async Task<IActionResult> LeaderReview()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LeaderReview(LeaderReviewDTO leaderReviewDTO)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "You need to log in to view your profile.";
                return RedirectToAction("Login", "Auth");
            }

            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            leaderReviewDTO.CreatedAt = DateTime.Now;
            leaderReviewDTO.CleanEventId = 2;
            leaderReviewDTO.LeaderId = "2";
            leaderReviewDTO.ReviewerId = userId;

            if (ModelState.IsValid)
            {
                ResponseDTO response = await _eventService.LeaderReview(leaderReviewDTO);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Leader review successfully!";
                    return RedirectToAction(nameof(LeaderReviewHistory));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(leaderReviewDTO);
        }
        public async Task<IActionResult> LeaderReviewHistory(string id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "You need to log in to view your profile.";
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
                TempData["success"] = "Leader review deleted successfully!";
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
                TempData["success"] = "Leader review updated successfully!";
                return RedirectToAction(nameof(LeaderReviewHistory));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(leaderReviewDTO);
        }

        //Dangky Volunteer
        [HttpGet]
        public IActionResult RegisterVolunteer(int eventId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

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
    }
}
