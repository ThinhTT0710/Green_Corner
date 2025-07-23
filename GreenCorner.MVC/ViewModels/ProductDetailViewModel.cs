using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.ViewModels
{
    public class ProductDetailViewModel
    {
        public ProductDTO Product { get; set; }
        public bool IsInWishlist { get; set; }
        public List<ProductDTO> BestSellingProducts { get; set; }
        public List<EventDTO> Top3OpenEvents { get; set; }

    }
}
