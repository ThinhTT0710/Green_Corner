using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace GreenCorner.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
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
    }
}
