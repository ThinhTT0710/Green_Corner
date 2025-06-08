namespace GreenCorner.MVC.Models
{
    public class EventReviewDTO
    {
        public int EventReviewId { get; set; }

        public int CleanEventId { get; set; }

        public string UserId { get; set; }

        public int? Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
