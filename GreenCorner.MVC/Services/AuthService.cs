using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;

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
    }
}
