using System.ComponentModel.DataAnnotations;

namespace GreenCorner.ChatAPI.DTOs
{
    public class NotificationDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; }
    }
}
