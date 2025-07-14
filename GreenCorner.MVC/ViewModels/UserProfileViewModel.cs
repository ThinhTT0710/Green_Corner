using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.ViewModels
{
    public class UserProfileViewModel
    {
        public int TotalPoints { get; set; }
        public UserDTO? User { get; set; }
        public List<PointTransactionDTO>? PointTransactions { get; set; }
        public List<OrderDTO>? OrderHistory { get; set; }
        public List<VolunteerDTO>? ParticipatedActivities { get; set; }
    }
}
