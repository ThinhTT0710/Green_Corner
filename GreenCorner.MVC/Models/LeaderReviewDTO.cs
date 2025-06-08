namespace GreenCorner.MVC.Models
{
    public class LeaderReviewDTO
    {
        public int LeaderReviewId { get; set; }

        public int CleanEventId { get; set; }

        public string LeaderId { get; set; } = null!;

        public string ReviewerId { get; set; } = null!;

        public int? Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
