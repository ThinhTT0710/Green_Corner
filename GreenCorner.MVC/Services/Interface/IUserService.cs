using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IUserService
    {
        Task<ResponseDTO?> GetUserById(string id);
        Task<ResponseDTO?> UpdateUser(UserDTO user);
        Task<ResponseDTO?> ChangePassword(ChangePasswordRequestDTO changePasswordRequest);
        Task<ResponseDTO?> GetAllUser();
        Task<ResponseDTO?> GetActiveUser();
        Task<ResponseDTO?> GetNearUser(string address);
        Task<ResponseDTO?> BanUser(string id);
        Task<ResponseDTO?> UnBanUser(string id);
    }
}
