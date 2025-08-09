using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.ViewModels
{
    public class EventReviewWithUserViewModel
    {
        public EventReviewDTO Review { get; set; } = null!;
        public UserDTO? Reviewer { get; set; }
    }
}
