using GreenCorner.AuthAPI.Models.DTO;
using GreenCorner.AuthAPI.Repositories.Interface;
using GreenCorner.AuthAPI.Services.Interface;

namespace GreenCorner.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            return await _authRepository.AssignRole(email, roleName);
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDto)
        {
            return await _authRepository.Login(loginRequestDto);
        }

        public async Task<string> Register(RegisterationRequestDTO registrationRequestDto)
        {
            return await _authRepository.Register(registrationRequestDto);
        }

        public async Task<LoginResponseDTO> LoginWithGoogle(GoogleLoginRequestDTO googleLoginRequest)
        {
            return await _authRepository.LoginWithGoogle(googleLoginRequest);
        }

        public async Task<LoginResponseDTO> LoginWithFacebook(FacebookLoginRequestDTO facebookLoginRequest)
        {
            return await _authRepository.LoginWithFacebook(facebookLoginRequest);
        }
    }
}
