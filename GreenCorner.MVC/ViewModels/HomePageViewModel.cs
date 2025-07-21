using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.ViewModels
{
    public class HomePageViewModel
    {
        public List<ProductDTO> BestSellingProducts { get; set; }
        public List<ProductDTO> NewestProducts { get; set; }

        public List<EventDTO> Top3OpenEvents { get; set; }
        public List<VoucherDTO> Top10Vouchers { get; set; }
    }
}
