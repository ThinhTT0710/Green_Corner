using System.ComponentModel.DataAnnotations;

namespace GreenCorner.MVC.Models
{
    public class LeaderReviewDTO
    {
        public int LeaderReviewId { get; set; }

        public int CleanEventId { get; set; }

        public string? LeaderId { get; set; } = null!;

        public string? ReviewerId { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn số sao đánh giá.")]
        [Range(1, 5, ErrorMessage = "Đánh giá phải từ 1 đến 5 sao.")]
        public int? Rating { get; set; }

        [Required(ErrorMessage = "Nội dung bình luận không được để trống.")]
        public string? Comment { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
