namespace GreenCorner.EcommerceAPI.Models.DTO
{
    public class MonthlyAnalyticsDto
    {
        public List<int> ProductsSold { get; set; } = new List<int>();
        public List<int> CompletedOrders { get; set; } = new List<int>();
        public List<int> OtherStatusOrders { get; set; } = new List<int>();
    }
}
