namespace GreenCorner.MVC.Models
{
    public class VolunteerDTO
    {
        public int VolunteerId { get; set; }

        public int CleanEventId { get; set; }

        public string UserId { get; set; } = null!;

        public string? ApplicationType { get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? Assignment { get; set; }

        public string? CarryItems { get; set; }
    }
}
