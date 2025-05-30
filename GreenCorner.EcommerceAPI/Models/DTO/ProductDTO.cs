namespace GreenCorner.EcommerceAPI.Models.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }

        public int? Discount { get; set; }

        public string? Category { get; set; }

        public string? Brand { get; set; }

        public string? Origin { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
