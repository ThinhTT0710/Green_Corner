namespace GreenCorner.BlogAPI.Models.DTOs
{
    public class BlogFavoriteAddDTO
    {
        public int BlogId { get; set; }

        public string UserId { get; set; } = null!;
    }
}
