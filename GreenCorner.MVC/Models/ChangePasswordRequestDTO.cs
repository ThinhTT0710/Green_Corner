using System.ComponentModel.DataAnnotations;

namespace GreenCorner.MVC.Models
{
    public class ChangePasswordRequestDTO
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        public string? UserID { get; set; }
        [Required(ErrorMessage = "Old password is required")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "New password is required")]
        [CustomValidation(typeof(ChangePasswordRequestDTO), "ValidateNewPassword")]
        public string NewPassword { get; set; }

        // function check if new password and old password are different
        public static ValidationResult ValidateNewPassword(string newPassword, ValidationContext validationContext)
        {
            var instance = (ChangePasswordRequestDTO)validationContext.ObjectInstance;
            if (instance.OldPassword == newPassword)
            {
                return new ValidationResult("New password and old password must be different");
            }
            return ValidationResult.Success;
        }
    }
}
