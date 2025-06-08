using GreenCorner.AuthAPI.Models.DTO;
using GreenCorner.AuthAPI.Repositories.Interface;
using GreenCorner.AuthAPI.Services.Interface;

namespace GreenCorner.AuthAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO> GetUserById(string userId)
        {
            return await _userRepository.GetUserById(userId);
        }

        public async Task<UserDTO> UpdateUser(UserDTO user)
        {
            return await _userRepository.UpdateUser(user);
        }

        public async Task<bool> ChangePassword(ChangePasswordRequestDTO changePasswordRequest)
        {
            return await _userRepository.ChangePassword(changePasswordRequest);
        }
        public async Task<bool> CheckPhoneNumber(string phoneNumber, string userId)
        {
            return await _userRepository.CheckPhoneNumber(phoneNumber, userId);
        }
    }
}
