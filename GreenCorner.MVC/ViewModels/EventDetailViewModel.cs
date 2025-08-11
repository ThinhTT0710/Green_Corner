using GreenCorner.MVC.Models;
using GreenCorner.MVC.Models.Chat;

namespace GreenCorner.MVC.ViewModels
{
    public class EventDetailViewModel
    {
        public EventDTO Event { get; set; }
        public ParticipationInfoResponse Participation { get; set; }
        public UserDTO TeamLeader { get; set; }
        public List<EventVolunteerInfo> Volunteers { get; set; }
        public List<ChatMessageDTO> ChatMessages { get; set; }
        public EventDetailViewModel()
        {
            Volunteers = new List<EventVolunteerInfo>();
        }
    }
    public class EventVolunteerInfo
    {
        public VolunteerDTO Volunteer { get; set; }
        public UserDTO User { get; set; }
    }
}
