
namespace GreenCorner.MVC.Models
{
    public class CartDTO
    {
        public int CartId { get; set; }

        public string UserId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public ProductDTO? Product { get; set; }
    }
}
