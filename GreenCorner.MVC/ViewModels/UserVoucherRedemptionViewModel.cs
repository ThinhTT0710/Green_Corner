using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.ViewModels
{
    public class UserVoucherRedemptionViewModel
    {
        public UserVoucherRedemptionDTO Redemption { get; set; } = new();
        public VoucherDTO Voucher { get; set; } = new();
    }
}
