using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace GreenCorner.MVC.Models
{
    public class FeedbackDTO
    {
        public int FeedBackId { get; set; }

        [ValidateNever]
        public string? UserId { get; set; }


        //public string UserId { get; set; } = null!;
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề.")]
        [StringLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung.")]
        [StringLength(1000, ErrorMessage = "Nội dung không được vượt quá 1000 ký tự.")]
        public string? Content { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
