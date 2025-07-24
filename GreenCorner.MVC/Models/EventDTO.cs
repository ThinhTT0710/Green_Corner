using System.ComponentModel.DataAnnotations;

namespace GreenCorner.MVC.Models
{
    public class EventDTO
    {
        public int CleanEventId { get; set; }

        [Required(ErrorMessage = "Tiêu đề sự kiện không được để trống.")]
        [StringLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Mô tả sự kiện không được để trống.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu không được để trống.")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc không được để trống.")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Range(1, 1000, ErrorMessage = "Số người tham gia tối đa phải từ 1 đến 1000.")]
        public int? MaxParticipants { get; set; }

        [Required(ErrorMessage = "Trạng thái sự kiện là bắt buộc.")]
        [StringLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự.")]
        public string? Status { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedAt { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Điểm thưởng phải là số không âm.")]
        public int? PointsAward { get; set; }

        [Required(ErrorMessage = "Địa chỉ sự kiện là bắt buộc.")]
        public string? Address { get; set; }

        [Url(ErrorMessage = "Đường dẫn hình ảnh không hợp lệ.")]
        public string? ImageUrl { get; set; }
    }
}
