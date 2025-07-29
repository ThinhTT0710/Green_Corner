using System.ComponentModel.DataAnnotations;

namespace GreenCorner.MVC.Models
{
    public class EventReviewDTO
    {
        public int EventReviewId { get; set; }

        [Required(ErrorMessage = "Mã sự kiện không được để trống.")]
        public int CleanEventId { get; set; }

        public string? UserId { get; set; } 

        [Range(0, 5, ErrorMessage = "Đánh giá phải từ 0 đến 5.")]
        public int? Rating { get; set; }

        [Required(ErrorMessage = "Bình luận không được để trống.")]
        public string? Comment { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }
    }
}
