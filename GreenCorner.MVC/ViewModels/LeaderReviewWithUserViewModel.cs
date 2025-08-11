using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.ViewModels
{
    public class LeaderReviewWithUserViewModel
    {
        public LeaderReviewDTO Review { get; set; } = null!;
        public UserDTO? Reviewer { get; set; }
        public UserDTO? Leader { get; set; }
    }
}
