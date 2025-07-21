namespace GreenCorner.MVC.Models.Admin
{
    public class EventMonthlyAnalyticsDto
    {
        public List<int> CompletedEvents { get; set; } = new List<int>();
        public List<int> CompletedTrashReports { get; set; } = new List<int>();
        public List<int> PendingTrashReports { get; set; } = new List<int>();
    }
}
