using GreenCorner.AuthAPI.Models.DTO;

namespace GreenCorner.AuthAPI.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<UserDTO> GetUserById(string userId);
        Task<UserDTO> UpdateUser(UserDTO user);
        Task<bool> ChangePassword(ChangePasswordRequestDTO changePasswordRequest);
        Task<bool> CheckPhoneNumber(string phoneNumber, string userId);
		Task<UserDTO> BanUser(string id);
		Task<UserDTO> UnBanUser(string id);
		Task<List<UserDTO>> GetAllUser();
	}
}
