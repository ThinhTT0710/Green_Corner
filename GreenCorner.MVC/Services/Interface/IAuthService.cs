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
		Task<ResponseDTO?> GetAllStaff();
		Task<ResponseDTO?> GetStaffById(string id);
		Task<ResponseDTO?> CreateStaff(StaffDTO staff);
        Task<ResponseDTO?> UpdateStaff(StaffDTO staff);
        Task<ResponseDTO?> BlockStaffAccount(string id);
		Task<ResponseDTO?> UnBlockStaffAccount(string id);


	}
}
