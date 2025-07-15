namespace GreenCorner.RewardAPI.Models.DTO
{
    public class UserVoucherRedemptionDTO
    {
        public int UserVoucherId { get; set; }

        public int VoucherId { get; set; }

        public string UserId { get; set; } = null!;

        public DateTime? RedeemedAt { get; set; }

    }
}
