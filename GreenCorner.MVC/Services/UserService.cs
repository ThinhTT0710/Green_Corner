using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;

namespace GreenCorner.MVC.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseService _baseService;
        public UserService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> ChangePassword(ChangePasswordRequestDTO changePasswordRequest)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.POST,
                Url = SD.AuthAPIBase + "/api/user",
                Data = changePasswordRequest
            });
        }

        public async Task<ResponseDTO?> GetUserById(string id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.AuthAPIBase + "/api/user/" + id,
            });
        }

        public async Task<ResponseDTO?> UpdateUser(UserDTO user)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.PUT,
                Url = SD.AuthAPIBase + "/api/user",
                Data = user,
            });
        }

        public async Task<ResponseDTO?> GetAllUser()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.AuthAPIBase + "/api/user/get-all-user"
            });
        }

        public async Task<ResponseDTO?> BanUser(string id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.AuthAPIBase + "/api/user/ban-user/" + id
            });
        }

        public async Task<ResponseDTO?> UnBanUser(string id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                APIType = SD.APIType.GET,
                Url = SD.AuthAPIBase + "/api/user/unban-user/" + id
            });
        }

    }
}
