using System.ComponentModel.DataAnnotations;

namespace GreenCorner.AuthAPI.Models.DTO
{
    public class ForgotPasswordRequestDTO
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
