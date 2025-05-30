using System.ComponentModel.DataAnnotations;

namespace GreenCorner.MVC.Models
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Please enter email address")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter Password")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$",
        ErrorMessage = "Password must contain at least 1 uppercase letter, 1 number and 1 special character.")]
        public string Password { get; set; }
    }
}
