namespace GreenCorner.BlogAPI.Models.DTOs
{
    public class BlogPostDTO
    {
        public int BlogId { get; set; }

        public string AuthorId { get; set; } = null!;

        public string? Title { get; set; }

        public string? Content { get; set; }

        public string? ThumbnailUrl { get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

    }
}
