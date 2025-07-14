namespace GreenCorner.RewardAPI.Models.DTO
{
    public class PointTransactionDTO
    {
        public int PointTransactionsId { get; set; }

        public string? UserId { get; set; } = null!;
        public int Points { get; set; }

        public string? Type { get; set; }

        public DateTime? CreatedAt { get; set; }
		public int Points { get; set; }
	}
}
