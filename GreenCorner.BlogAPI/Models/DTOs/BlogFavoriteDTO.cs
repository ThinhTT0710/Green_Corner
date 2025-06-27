namespace GreenCorner.BlogAPI.Models.DTOs
{
    public class BlogFavoriteDTO
    {
        public int BlogFavoriteId { get; set; }

        public int BlogId { get; set; }

        public string UserId { get; set; } = null!;

        public BlogPostDTO Blog { get; set; } = null!;
    }
}
