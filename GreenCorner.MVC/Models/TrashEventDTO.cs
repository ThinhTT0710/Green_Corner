

using System.ComponentModel.DataAnnotations;

namespace GreenCorner.MVC.Models;

public partial class TrashEventDTO
{
    public int TrashReportId { get; set; }

    public string UserId { get; set; } = null!;

    public string? Location { get; set; }

    public string? Address { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }
}
