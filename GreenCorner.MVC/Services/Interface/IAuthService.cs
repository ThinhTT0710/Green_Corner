using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IAuthService
    {
        Task<ResponseDTO?> LoginAsync(LoginRequestDTO loginRequest);
        Task<ResponseDTO?> RegisterAsync(RegisterationRequestDTO registerationRequest);
        Task<ResponseDTO?> AssignRoleAsync(RegisterationRequestDTO registerationRequest);
        Task<ResponseDTO?> ConfirmEmailAsync(string userID, string token);
        Task<ResponseDTO?> EmailForgotPasswordAsync(string email);
        Task<ResponseDTO?> ForgotPasswordAsync(ForgotPasswordRequestDTO forgotPasswordRequest);
        Task<ResponseDTO?> ResendConfirmEmailAsync(string email);
        Task<ResponseDTO?> LoginWithGoogleAsync(GoogleLoginRequestDTO googleLoginRequest);
        Task<ResponseDTO?> LoginWithFacebookAsync(FacebookLoginRequestDTO facebookLoginRequest);
    }
}
