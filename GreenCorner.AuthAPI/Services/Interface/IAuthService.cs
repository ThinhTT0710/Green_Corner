using GreenCorner.AuthAPI.Models.DTO;

namespace GreenCorner.AuthAPI.Services.Interface
{
    public interface IAuthService
    {
        Task<string> Register(RegisterationRequestDTO registrationRequestDto);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDto);
        Task<bool> AssignRole(string email, string roleName);

    }
}
