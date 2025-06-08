using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace GreenCorner.MVC.Models
{
    public class ReportDTO
    {
        public int ReportId { get; set; }

        [ValidateNever]
        public string? LeaderId { get; set; }
        //public string LeaderId { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề báo cáo.")]
        [StringLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung báo cáo.")]
        [StringLength(1000, ErrorMessage = "Nội dung không được vượt quá 1000 ký tự.")]
        public string? Content { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
