using GreenCorner.MVC.Models;
using GreenCorner.MVC.Models.Admin;

namespace GreenCorner.MVC.ViewModels
{
    public class EventAnalyticViewModel
    {
        public int TotalEvent { get; set; }
        public int TotalOpenEvent { get; set; }
        public int TotalVolunteer { get; set; }
        public int TotalPedingVolunteer { get; set; }
        public List<EventDTO> EventOpenList { get; set; }
        public List<TrashEventDTO> TrashEventList{ get; set; }
        public EventMonthlyAnalyticsDto ChartData { get; set; }
    }
}
