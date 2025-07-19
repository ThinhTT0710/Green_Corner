using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.ViewModels
{
    public class RewardPointWithUserViewModel
    {
        public RewardPointDTO Reward { get; set; }
        public UserDTO? User { get; set; }
    }
}
