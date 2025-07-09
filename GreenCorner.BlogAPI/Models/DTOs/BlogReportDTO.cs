namespace GreenCorner.BlogAPI.Models.DTOs
{
    public class BlogReportDTO
    {
        public int BlogReportId { get; set; }

        public int BlogId { get; set; }

        public string? Reason { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string UserId { get; set; } = null!;

        //public BlogPostDTO Blog { get; set; } = null!;
    }
}
