namespace GreenCorner.MVC.ViewModels
{
	public class SystemLogViewModel
	{
			public int Id { get; set; }
			public string UserId { get; set; }
			public string ActionType { get; set; }
			public string ObjectType { get; set; }
			public int ObjectId { get; set; }
			public string Description { get; set; }
			public DateTime CreatedAt { get; set; }

			public string UserName { get; set; }
			public string FullName { get; set; }
			public string Address { get; set; }
			public string Avatar { get; set; }
			public string Email { get; set; }
			public bool IsBanned { get; set; }
	}
}
