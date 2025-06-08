namespace GreenCorner.BlogAPI.Models.DTOs
{
    public class ReportDTO
    {
        public int ReportId { get; set; }

        public string LeaderId { get; set; } = null!;

        public string? Title { get; set; }

        public string? Content { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
