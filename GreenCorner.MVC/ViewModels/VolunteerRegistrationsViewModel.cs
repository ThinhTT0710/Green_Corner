using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.ViewModels
{
    public class VolunteerWithUserViewModel
    {
        public VolunteerDTO Volunteer { get; set; }
        public UserDTO User { get; set; }
    }

    public class VolunteerRegistrationsViewModel
    {
        public List<VolunteerWithUserViewModel> Registrations { get; set; }
    }
}
