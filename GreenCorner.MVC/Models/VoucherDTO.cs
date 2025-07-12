namespace GreenCorner.MVC.Models
{
    public class VoucherDTO
    {
        public int VoucherId { get; set; }
        public string? Title { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public int? PointsRequired { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}
