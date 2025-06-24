using System.ComponentModel.DataAnnotations;

namespace GreenCorner.MVC.Models
{
    public class BlogPostDTO
    {
        public int BlogId { get; set; }

        [Required(ErrorMessage = "Tác giả là bắt buộc.")]
        public string AuthorId { get; set; } = null!;

        [Required(ErrorMessage = "Tiêu đề là bắt buộc.")]
        [StringLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Nội dung là bắt buộc.")]
        public string? Content { get; set; }

        public string? ThumbnailUrl { get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
