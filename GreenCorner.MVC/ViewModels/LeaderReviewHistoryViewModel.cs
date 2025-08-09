using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.ViewModels
{
    public class LeaderReviewHistoryViewModel
    {
        public LeaderReviewDTO LeaderReview { get; set; }
        public EventDTO? Event { get; set; }
        public UserDTO? Leader { get; set; }
    }
}
