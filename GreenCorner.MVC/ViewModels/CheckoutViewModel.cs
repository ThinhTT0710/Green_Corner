using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.ViewModels
{
    public class CheckoutViewModel
    {

        public List<CartDTO> CartItems { get; set; }
        public OrderDTO Order { get; set; }
    }
}
