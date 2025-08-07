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
                    TempData["success"] = "Đăng nhập thành công.";
                    var role = User.Claims.Where(u => u.Type == ClaimTypes.Role)?.FirstOrDefault()?.Value;
                    if (role == SD.RoleAdmin || role == SD.RoleSaleStaff || role == SD.RoleEventStaff)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
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

            if (result != null && result.IsSuccess)
            {
                assignRole = await _authService.AssignRoleAsync(registerationRequest);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Đăng ký thành công. Vui lòng xác nhận email trước khi đăng nhập.";
                    return RedirectToAction("Login");
                }
                TempData["error"] = "Đăng ký không thành công. Vui lòng thử lại.";
            }
            TempData["error"] = result.Message;
            return View(registerationRequest);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                TempData["error"] = "Yêu cầu xác nhận không hợp lệ.";
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
                TempData["success"] = "Đăng nhập với Google thành công.";
                return RedirectToAction("Index", "Home");
            }

            TempData["error"] = response?.Message ?? "Đăng nhập với Google không thành công.";
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
                TempData["success"] = "Đăng nhập với Facebook thành công.";
                return Redirect(returnUrl);
            }

            TempData["error"] = response?.Message ?? "Đăng nhập với Facebook không thành công.";
            return RedirectToAction("Login");
        }
        private async Task SignInUser(LoginResponseDTO loginResponse)
        {

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(loginResponse.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var securityStampClaim = jwt.Claims.FirstOrDefault(u => u.Type == "security_stamp");
            if (securityStampClaim != null)
            {
                identity.AddClaim(new Claim("security_stamp", securityStampClaim.Value));
            }

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            HttpContext.User = principal;
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
                TempData["loginError"] = "Vui lòng đăng nhập!";
                return RedirectToAction("Login", "Auth");
            }

            var file = Request.Form.Files.FirstOrDefault(); 

            if (file != null && file.Length > 0)
            {
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/admin/imgs/avatars");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                staffDTO.Avatar = fileName;
            }
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
            staffDTO.ID = userId;
            if (ModelState.IsValid)
            {
                ResponseDTO response = await _authService.CreateStaff(staffDTO);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Tạo nhân viên thành công!";
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
            var file = Request.Form.Files.FirstOrDefault(); 
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

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                if (existingStaff.Avatar != fileName)
                {
                    existingStaff.Avatar = fileName;
                }
                staffDTO.Avatar = existingStaff.Avatar;
            }

            ResponseDTO response = await _authService.UpdateStaff(staffDTO);

            if (response != null && response.IsSuccess)
            {

                TempData["success"] = "Cập nhật thông tin nhân viên thành công!";
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
                TempData["success"] = "Đã khóa tài khoản thành công.";
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
                TempData["success"] = "Đã mở khóa tài khoản thành công.";
                return RedirectToAction(nameof(GetStaffList));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
