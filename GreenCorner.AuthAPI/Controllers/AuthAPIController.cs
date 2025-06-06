using GreenCorner.AuthAPI.Models;
using GreenCorner.AuthAPI.Models.DTO;
using GreenCorner.AuthAPI.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GreenCorner.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        protected ResponseDTO _response;
        public AuthAPIController(IAuthService authService, UserManager<User> userManager, IEmailService emailService)
        {
            _authService = authService;
            this._response = new ResponseDTO();
            _userManager = userManager;
            _emailService = emailService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO registerRequest) 
        {
            var response = await _authService.Register(registerRequest);
            if (!string.IsNullOrEmpty(response))
            {
                _response.Message = response;
                _response.IsSuccess = false;
                _response.Result = null;
                return BadRequest(_response);
            }
            var userEntity = await _userManager.FindByEmailAsync(registerRequest.Email);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(userEntity);
            var encodeToken = Base64UrlEncoder.Encode(token);
            var confirmationLink = $"https://localhost:7000/Auth/ConfirmEmail?userId={userEntity.Id}&token={encodeToken}";

            await _emailService.SendEmailAsync(registerRequest.Email, "Verify Your Email", $"<h1>Welcome to GreenCorner</h1><p>Please confirm your email by <a href='{confirmationLink}'>clicking here</a></p>");
            _response.Message = "Account created successfully, please check email.";
            return Ok(_response);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {   var loginResponse = await _authService.Login(loginRequest);
            if(loginResponse.User == null)
            { _response.IsSuccess = false;
                _response.Message = "Email or password is incorrect";
                return BadRequest(_response);
            }
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            bool isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed)
            {
                _response.Message = "Please confirm your email before logging in.";
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDTO googleLoginRequest)
        {
            var loginResponse = await _authService.LoginWithGoogle(googleLoginRequest);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Google login failed.";
                return BadRequest(_response);
            }

            _response.Result = loginResponse;
            return Ok(_response);

        }

        [HttpPost("facebook-login")]
        public async Task<IActionResult> FacebookLogin([FromBody] FacebookLoginRequestDTO facebookLoginRequest)
        {
            var loginResponse = await _authService.LoginWithFacebook(facebookLoginRequest);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Facebook login failed.";
                return BadRequest(_response);
            }

            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] RegisterationRequestDTO model)
        {
            var assignRoleSuccess = await _authService.AssignRole(model.Email, model.RoleName.ToUpper());
            if (!assignRoleSuccess)
            {
                _response.IsSuccess = false;
                _response.Message = "Failed to assign role";
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid confirmation request.";
                return BadRequest(_response);
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "User not found.";
                return NotFound(_response);
            }
            var decodedToken = Base64UrlEncoder.Decode(token);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (result.Succeeded)
            {
                _response.IsSuccess = true;
                _response.Message = "Email confirmed successfully.";
                return Ok(_response);
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid confirmation request.";
                return BadRequest(_response);
            }
        }

        [HttpGet("resend-confirm-email")]
        public async Task<IActionResult> ResendConfirmEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "User not found.";
                return NotFound(_response);
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodeToken = Base64UrlEncoder.Encode(token);
            var confirmationLink = $"https://localhost:7000/Auth/ConfirmEmail?userId={user.Id}&token={encodeToken}";

            await _emailService.SendEmailAsync(email, "Confirm your email", $"<h1>Welcome to GreenCorner</h1><p>Please confirm your email by <a href='{confirmationLink}'>clicking here</a></p>");
            _response.Message = "Email sent successfully, please check email.";
            return Ok(_response);
        }

        [HttpGet("email-forgot-password")]
        public async Task<IActionResult> EmailForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "User not found.";
                return NotFound(_response);
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodeToken = Base64UrlEncoder.Encode(token);
            var forgotPasswordLink = $"https://localhost:7000/Auth/ForgotPassword?userId={user.Id}&token={encodeToken}";

            await _emailService.SendEmailAsync(email, "Reset your password", $"<h1>Welcome to GreenCorner</h1><p>Please reset your password by <a href='{forgotPasswordLink}'>clicking here</a></p>");
            _response.Message = "Email sent successfully, please check email.";
            return Ok(_response);
        }

        [HttpPost("fotgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDTO forgotPasswordRequest)
        {
            var user = await _userManager.FindByIdAsync(forgotPasswordRequest.UserId);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "User not found.";
                return NotFound(_response);
            }
            var decodedToken = Base64UrlEncoder.Decode(forgotPasswordRequest.Token);
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, forgotPasswordRequest.Password);
            if (result.Succeeded)
            {
                _response.IsSuccess = true;
                _response.Message = "Password reset successfully.";
                return Ok(_response);
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Failed to reset password.";
                return BadRequest(_response);
            }
        }
    }
}
