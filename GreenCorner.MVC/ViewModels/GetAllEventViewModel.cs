using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.ViewModels
{
    public class GetAllEventViewModel
    {
        public EventDTO Event { get; set; }
        public ParticipationInfoResponse Participation { get; set; }
        public string? TeamLeaderId { get; set; }
    }
}
