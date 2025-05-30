using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GreenCorner.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequest = new LoginRequestDTO();
            return View(loginRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequest)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO response = await _authService.LoginAsync(loginRequest);
                if (response != null && response.IsSuccess)
                {
                    LoginResponseDTO loginResponse = JsonConvert.DeserializeObject<LoginResponseDTO>(response.Result.ToString());
                    await SignInUser(loginResponse);
                    _tokenProvider.SetToken(loginResponse.Token);
                    TempData["success"] = "Login successfully.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["error"] = response.Message;
                }
            }
            return View(loginRequest);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterationRequestDTO registerationRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(registerationRequest);
            }
            registerationRequest.RoleName = SD.RoleCustomer;
            ResponseDTO result = await _authService.RegisterAsync(registerationRequest);
            ResponseDTO assignRole;

            if(result != null && result.IsSuccess) 
            { 
                assignRole = await _authService.AssignRoleAsync(registerationRequest);
                if(assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Register successfully. Please confirm email before logging in.";
                    return RedirectToAction("Login");
                }
                TempData["error"] = "Register failed. Please try again.";
            }
            TempData["error"] = result.Message;
            return View(registerationRequest);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                TempData["error"] = "Invalid confirmation request.";
                return RedirectToAction("Login");
            }
            var result = await _authService.ConfirmEmailAsync(userId, token);
            if (result != null && result.IsSuccess)
            {
                TempData["success"] = result.Message;
                return RedirectToAction("Login");
            }
            TempData["error"] = result.Message;
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Logout() 
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDTO loginResponse) 
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(loginResponse.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));


            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);       
        }
    }
}
