namespace GreenCorner.MVC.Models
{
    public class OrderDetailDTO
    {
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public ProductDTO? ProductDTO { get; set; }
    }
}
