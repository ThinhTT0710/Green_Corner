using System.ComponentModel.DataAnnotations;

namespace GreenCorner.MVC.Models
{
    public class ForgotPasswordRequestDTO
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        [Required(ErrorMessage = "Please enter password.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$",
        ErrorMessage = "Password must contain at least 1 uppercase letter, 1 number and 1 special character.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter confirm password.")]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
