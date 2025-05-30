using GreenCorner.AuthAPI.Models.DTO;

namespace GreenCorner.AuthAPI.Repositories.Interface
{
    public interface IAuthRepository
    {
        Task<string> Register(RegisterationRequestDTO registrationRequestDto);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDto);
        Task<bool> AssignRole(string email, string roleName);
    }
}
