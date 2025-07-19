using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.ViewModels
{
    public class UserWithVoucherRedemptionViewModel
    {
        public UserDTO User { get; set; }
        public UserVoucherRedemptionDTO Redemption { get; set; }
    }
}
