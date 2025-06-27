using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.ViewModels
{
    public class VolunteerWithEventViewModel
    {
        public VolunteerDTO Volunteer { get; set; }
        public EventDTO Event { get; set; }
    }
    public class VolunteerEventListViewModel
    {
        public List<VolunteerWithEventViewModel> Participations { get; set; }
    }
}
