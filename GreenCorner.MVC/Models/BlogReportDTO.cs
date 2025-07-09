using System.ComponentModel.DataAnnotations;

namespace GreenCorner.MVC.Models
{
    public class BlogReportDTO
    {
        public int BlogReportId { get; set; }

        [Required(ErrorMessage = "BlogId là bắt buộc.")]
        public int BlogId { get; set; }

        [Required(ErrorMessage = "Lý do báo cáo là bắt buộc.")]
        [StringLength(500, ErrorMessage = "Lý do không được vượt quá 500 ký tự.")]
        public string? Reason { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string UserId { get; set; } = null!;
    }
}
