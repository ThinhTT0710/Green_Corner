namespace GreenCorner.RewardAPI.Models.DTO
{
    public class VoucherDTO
    {
        public int VoucherId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int PointsRequired { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Code { get; set; }
        public string? Address { get; set; }

        public int Quantity { get; set; }

        public bool IsActive { get; set; }
    }

}
