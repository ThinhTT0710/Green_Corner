using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenCorner.AuthAPI.Models
{
	public class SystemLog
	{
		public int Id { get; set; }

		public string UserId { get; set; } 

		public string ActionType { get; set; }

		public string ObjectType { get; set; }

		public int ObjectId { get; set; }

		public string Description { get; set; }

		public DateTime CreatedAt { get; set; }

		[ForeignKey("UserId")]
		public virtual User? User { get; set; }
	}
}
