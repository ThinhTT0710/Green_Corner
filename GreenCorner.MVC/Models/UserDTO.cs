
using System.ComponentModel.DataAnnotations;

namespace GreenCorner.MVC.Models
{
    public class UserDTO
    {
        public string? ID { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Ảnh đại diện là bắt buộc.")]
        [Url(ErrorMessage = "Đường dẫn ảnh không hợp lệ.")]
        public string Avatar { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string PhoneNumber { get; set; }

        public bool IsBan { get; set; } = false;
    }
}
