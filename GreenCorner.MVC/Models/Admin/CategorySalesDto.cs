namespace GreenCorner.MVC.Models.Admin
{
    public class CategorySalesDto
    {
        public List<string> CategoryNames { get; set; } = new List<string>();
        public List<int> QuantitiesSold { get; set; } = new List<int>();
    }
}
