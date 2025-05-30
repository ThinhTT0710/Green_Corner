using System.ComponentModel.DataAnnotations;

namespace GreenCorner.MVC.Models
{
    public class RegisterationRequestDTO
    {
        [Required(ErrorMessage = "Please enter full name")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Please enter email address")]
        [EmailAddress(ErrorMessage = "Please enter correct email format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter address")]
        public string Address { get; set; }
        public string Avatar { get; set; } = "default.png";
        [Required(ErrorMessage = "Please enter phone number")]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Please enter correct phone number format")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please enter Password")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$",
        ErrorMessage = "Password must contain at least 1 uppercase letter, 1 number and 1 special character.")]
        public string Password { get; set; }
        public string? RoleName { get; set; }
    }
}
