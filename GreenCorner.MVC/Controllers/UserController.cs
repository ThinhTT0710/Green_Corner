using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace GreenCorner.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IVolunteerService _volunteerService;
        private readonly IEventService _eventService;
        public UserController(IUserService userService, IVolunteerService volunteerService, IEventService eventService)
        {
            _userService = userService;
            _volunteerService = volunteerService;
            _eventService = eventService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "You need to log in to view your profile.";
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdateProfile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "You need to log in to view your profile.";
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

            var response = await _userService.UpdateUser(user);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Profile updated successfully.";
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
                    TempData["success"] = "Change Password Successfully!";
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

            var vm = new VolunteerEventListViewModel
            {
                Participations = list
            };

            return View(vm);
        }
    }
}
