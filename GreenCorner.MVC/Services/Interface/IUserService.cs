using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IUserService
    {
        Task<ResponseDTO?> GetUserById(string id);
        Task<ResponseDTO?> UpdateUser(UserDTO user);
        Task<ResponseDTO?> ChangePassword(ChangePasswordRequestDTO changePasswordRequest);
    }
}
