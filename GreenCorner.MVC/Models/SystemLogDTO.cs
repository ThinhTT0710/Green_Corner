namespace GreenCorner.MVC.Models
{
	public class SystemLogDTO
	{
			public int Id { get; set; }

			public string UserId { get; set; }

			public string ActionType { get; set; }

			public string ObjectType { get; set; }

			public int ObjectId { get; set; }

			public string Description { get; set; }

			public DateTime CreatedAt { get; set; }

			public virtual User? User { get; set; }
	}
}
