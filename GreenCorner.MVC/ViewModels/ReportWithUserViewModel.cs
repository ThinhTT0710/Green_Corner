using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.ViewModels
{
    public class ReportWithUserViewModel
    {
        public ReportDTO Report { get; set; }
        public UserDTO? User { get; set; }
    }
}
