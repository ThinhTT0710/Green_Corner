namespace GreenCorner.EcommerceAPI.Models.DTO
{
    public class WishListDTO
    {
        public int WishListId { get; set; }

        public string UserId { get; set; }

        public int ProductId { get; set; }

        public ProductDTO? Product { get; set; }
    }
}
