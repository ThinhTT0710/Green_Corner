namespace GreenCorner.MVC.Models
{
    public class WishListDTO
    {
        public int WishListId { get; set; }

        public string UserId { get; set; } 

        public int ProductId { get; set; }

        public ProductDTO? Product { get; set; }
    }
}
