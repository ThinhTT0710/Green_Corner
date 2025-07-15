using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.ViewModels
{
    public class HomePageViewModel
    {
        public List<ProductDTO> BestSellingProducts { get; set; }
        public List<ProductDTO> NewestProducts { get; set; }
    }
}
