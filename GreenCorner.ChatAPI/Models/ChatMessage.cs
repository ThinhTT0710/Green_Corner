using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GreenCorner.ChatAPI.Models
{
    public class ChatMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChatMessageId { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        [MaxLength(255)]
        public string SenderName { get; set; }

        [MaxLength(255)]
        public string? SenderAvatar { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}
