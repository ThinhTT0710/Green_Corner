using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
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
        public IActionResult ResendConfirmEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResendConfirmEmail(IFormCollection form) 
        {
            var email = form["email-forgot"];
            var response = await _authService.ResendConfirmEmailAsync(email);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response.Message;
                return RedirectToAction("Login");
            }
            return View(form);
        }

        [HttpGet]
        public IActionResult EmailForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailForgotPassword(IFormCollection form) 
        {
            var email = form["email-forgot"];
            var response = await _authService.EmailForgotPasswordAsync(email);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response.Message;
                return RedirectToAction("Login");
            }
            return View(form);
        }

        [HttpGet]
        public IActionResult ForgotPassword(string userId, string token) 
        {
            var forgotpasswordRequest = new ForgotPasswordRequestDTO
            {
                UserId = userId,
                Token = token
            };
            return View(forgotpasswordRequest);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDTO forgotPasswordRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(forgotPasswordRequest);
            }
            var response = await _authService.ForgotPasswordAsync(forgotPasswordRequest);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response.Message;
                return RedirectToAction("Login");
            }
            TempData["error"] = response.Message;
            return View(forgotPasswordRequest);
        }

        [HttpGet]
        public async Task<IActionResult> Logout() 
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult GoogleLogin()
        {
                var properties = new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                };
                return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal?.Identities?.FirstOrDefault()?.Claims;

            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var picture = claims?.FirstOrDefault(c => c.Type == "picture")?.Value;

            // Tạo request gửi về API
            var response = await _authService.LoginWithGoogleAsync(new GoogleLoginRequestDTO
            {
                Email = email,
                FullName = name,
                Avatar = picture
            });

            if (response != null && response.IsSuccess)
            {
                var loginResponse = JsonConvert.DeserializeObject<LoginResponseDTO>(response.Result.ToString());
                await SignInUser(loginResponse);
                _tokenProvider.SetToken(loginResponse.Token);
                TempData["success"] = "Login with Google successfully.";
                return RedirectToAction("Index", "Home");
            }

            TempData["error"] = response?.Message ?? "Login failed.";
            return RedirectToAction("Login", "Auth");
        }


        public IActionResult LoginFacebook(string returnUrl = "/")
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("FacebookCallback", new { returnUrl })
            };

            return Challenge(properties, "Facebook");
        }
        public async Task<IActionResult> FacebookCallback(string returnUrl = "/")
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return RedirectToAction("Login");

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var fullName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var avatar = claims?.FirstOrDefault(c => c.Type == "picture")?.Value ?? "";

            var loginRequest = new FacebookLoginRequestDTO
            {
                Email = email,
                FullName = fullName,
                Avatar = avatar
            };

            var response = await _authService.LoginWithFacebookAsync(loginRequest);
            if (response != null && response.IsSuccess)
            {
                var loginResponse = JsonConvert.DeserializeObject<LoginResponseDTO>(response.Result.ToString());
                await SignInUser(loginResponse);
                _tokenProvider.SetToken(loginResponse.Token);
                return Redirect(returnUrl);
            }

            TempData["error"] = response?.Message ?? "Login failed.";
            return RedirectToAction("Login");
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
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

		public async Task<IActionResult> GetStaffList()
		{
			List<UserDTO> listStaff = new();
			ResponseDTO? response = await _authService.GetAllStaff();
			if (response != null && response.IsSuccess)
			{
				listStaff = JsonConvert.DeserializeObject<List<UserDTO>>(response.Result.ToString());
			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return View(listStaff);
		}
        
		public async Task<IActionResult> CreateStaff()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateStaff(StaffDTO staffDTO)
		{
			if (!User.Identity.IsAuthenticated)
			{
				TempData["loginError"] = "You need to log in to view your profile.";
				return RedirectToAction("Login", "Auth");
			}

            var file = Request.Form.Files.FirstOrDefault(); // Lấy file đầu tiên

            if (file != null && file.Length > 0)
            {
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/admin/imgs/avatar-staff");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Tạo tên file duy nhất
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                // Lưu file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Gán tên file vào thuộc tính
                staffDTO.Avatar = fileName;
            }
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            staffDTO.ID = userId;
			if (ModelState.IsValid)
			{
				ResponseDTO response = await _authService.CreateStaff(staffDTO);
				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Create Staff successfully!";
					return RedirectToAction(nameof(GetStaffList));
				}
				else
				{
					TempData["error"] = response?.Message;
				}
			}
			return View(staffDTO);
		}
        public async Task<IActionResult> UpdateStaff(string userId)
        {
            ResponseDTO response = await _authService.GetStaffById(userId);
            if (response != null && response.IsSuccess)
            {
                StaffDTO staffDTO = JsonConvert.DeserializeObject<StaffDTO>(response.Result.ToString());
                return View(staffDTO);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStaff(StaffDTO staffDTO)
        {

            ResponseDTO? existingResponse = await _authService.GetStaffById(staffDTO.ID);
            StaffDTO existingStaff = JsonConvert.DeserializeObject<StaffDTO>(existingResponse.Result.ToString());
            var file = Request.Form.Files.FirstOrDefault(); // Lấy file đầu tiên
            staffDTO.Avatar = existingStaff.Avatar;
            staffDTO.Password = existingStaff.Password;
            staffDTO.Role = existingStaff.Role;
            if (file != null && file.Length > 0)
            {
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/admin/imgs/avatar-staff");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Tạo tên file duy nhất
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                // Lưu file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                if(existingStaff.Avatar!= fileName)
                {
                    existingStaff.Avatar = fileName;
                }
                // Gán tên file vào thuộc tính
                staffDTO.Avatar = existingStaff.Avatar;
            }
            
            ResponseDTO response = await _authService.UpdateStaff(staffDTO);

            if (response != null && response.IsSuccess)
            {
                
                TempData["success"] = "Staff updated successfully!";
                return RedirectToAction(nameof(GetStaffList));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(staffDTO);
        }
        public async Task<IActionResult> BlockStaff(string staffID)
		{
            ResponseDTO response = await _authService.BlockStaffAccount(staffID);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Block account successfully.";
                return RedirectToAction(nameof(GetStaffList));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

		public async Task<IActionResult> UnBlockStaff(string staffID)
		{
			ResponseDTO response = await _authService.UnBlockStaffAccount(staffID);
			if (response != null && response.IsSuccess)
			{
                TempData["success"] = "Unblock account successfully.";
                return RedirectToAction(nameof(GetStaffList));
			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return NotFound();
		}


	}
}
