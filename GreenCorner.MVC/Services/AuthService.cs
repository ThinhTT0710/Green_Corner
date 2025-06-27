using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;
using Newtonsoft.Json.Linq;

namespace GreenCorner.MVC.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO?> AssignRoleAsync(RegisterationRequestDTO registerationRequest)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Url = SD.AuthAPIBase + "/api/auth/assign-role",
                Data = registerationRequest

            });
        }

        public async Task<ResponseDTO?> LoginAsync(LoginRequestDTO loginRequest)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Url = SD.AuthAPIBase + "/api/auth/login",
                Data = loginRequest

            }, withBearer: false);
        }

        public async Task<ResponseDTO?> RegisterAsync(RegisterationRequestDTO registerationRequest)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Url = SD.AuthAPIBase + "/api/auth/register",
                Data = registerationRequest
            }, withBearer: false);
        }

        public async Task<ResponseDTO?> ConfirmEmailAsync(string userID, string token)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.AuthAPIBase + "/api/auth/confirm-email?userId=" + userID + "&token=" + token
            });
        }

        public async Task<ResponseDTO?> EmailForgotPasswordAsync(string email)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.AuthAPIBase + "/api/auth/email-forgot-password?email=" + email
            });
        }

        public async Task<ResponseDTO?> ForgotPasswordAsync(ForgotPasswordRequestDTO forgotPasswordRequest)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Url = SD.AuthAPIBase + "/api/auth/fotgot-password",
                Data = forgotPasswordRequest
            });
        }

        public async Task<ResponseDTO?> ResendConfirmEmailAsync(string email)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.AuthAPIBase + "/api/auth/resend-confirm-email?email=" + email
            });
        }

        public async Task<ResponseDTO?> LoginWithGoogleAsync(GoogleLoginRequestDTO googleLoginRequest)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Url = SD.AuthAPIBase + "/api/auth/google-login",
                Data = googleLoginRequest
            }, withBearer: false);
        }

        public async Task<ResponseDTO?> LoginWithFacebookAsync(FacebookLoginRequestDTO facebookLoginRequest)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Url = SD.AuthAPIBase + "/api/auth/facebook-login",
                Data = facebookLoginRequest
            }, withBearer: false);
        }

		public async Task<ResponseDTO?> GetAllStaff()
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				APIType = SD.APIType.GET,
				Url = SD.AuthAPIBase + "/api/Admin"
			});
		}

		public async Task<ResponseDTO?> GetStaffById(string id)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				APIType = SD.APIType.GET,
				Url = SD.AuthAPIBase + "/api/Admin/"+id
			});
		}

		public async Task<ResponseDTO?> CreateStaff(StaffDTO staff)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				APIType = SD.APIType.POST,
				Data = staff,
				Url = SD.AuthAPIBase + "/api/Admin"
			});
		}
        public async Task<ResponseDTO?> UpdateStaff(StaffDTO staff)
        {
             
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.PUT,
                Data = staff,
                Url = SD.AuthAPIBase + "/api/Admin/update-staff"
			});
        }

        public async Task<ResponseDTO?> BlockStaffAccount(string id)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				APIType = SD.APIType.GET,
				Url = SD.AuthAPIBase + "/api/Admin/block-staff/" + id
			});
		}

		public async Task<ResponseDTO?> UnBlockStaffAccount(string id)
		{
			return await _baseService.SendAsync(new RequestDTO
			{
				APIType = SD.APIType.GET,
				Url = SD.AuthAPIBase + "/api/Admin/unblock-staff/" + id
			});
		}
	}
}
