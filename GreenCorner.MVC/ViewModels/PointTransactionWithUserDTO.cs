using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.ViewModels
{
    public class PointTransactionWithUserDTO
    {
        public PointTransactionDTO Transaction { get; set; }
        public UserDTO User { get; set; }
    }
}
