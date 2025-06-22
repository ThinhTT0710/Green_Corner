namespace GreenCorner.BlogAPI.Models.DTOs
{
    public class FeedbackDTO
    {
        public int FeedBackId { get; set; }

        public string UserId { get; set; } = null!;

        public string? Title { get; set; }

        public string? Content { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
