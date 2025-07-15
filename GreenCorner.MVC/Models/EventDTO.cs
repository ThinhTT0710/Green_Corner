namespace GreenCorner.MVC.Models
{
    public class EventDTO
    {
        public int CleanEventId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? MaxParticipants { get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedAt { get; set; }
        public int? PointsAward { get; set; }
        public string? Address { get; set; }

        public string? ImageUrl { get; set; }
    }
}
