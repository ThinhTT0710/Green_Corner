using System.ComponentModel.DataAnnotations;

namespace GreenCorner.MVC.Models
{
    public class VolunteerDTO
    {
        public int VolunteerId { get; set; }

        [Required(ErrorMessage = "Sự kiện không được để trống.")]
        public int CleanEventId { get; set; }

        [Required(ErrorMessage = "Người dùng không được để trống.")]
        public string UserId { get; set; } = null!;

        [StringLength(50, ErrorMessage = "Loại đơn đăng ký không được vượt quá 50 ký tự.")]
        public string? ApplicationType { get; set; }

        [StringLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự.")]
        public string? Status { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Thời gian tạo không hợp lệ.")]
        public DateTime? CreatedAt { get; set; }

        [StringLength(100, ErrorMessage = "Phân công không được vượt quá 100 ký tự.")]
        public string? Assignment { get; set; }

        [StringLength(100, ErrorMessage = "Vật dụng mang theo không được vượt quá 100 ký tự.")]
        public string? CarryItems { get; set; }
    }
}
