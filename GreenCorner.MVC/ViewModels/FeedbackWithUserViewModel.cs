using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.ViewModels
{
    public class FeedbackWithUserViewModel
    {
        public FeedbackDTO Feedback { get; set; }
        public UserDTO? User { get; set; }
    }
}
