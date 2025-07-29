

using System.ComponentModel.DataAnnotations;

namespace GreenCorner.MVC.Models;

public partial class TrashEventDTO
{
    public int TrashReportId { get; set; }

    public string UserId { get; set; } = null!;
    [Required(ErrorMessage = "Vui lòng cung cấp tọa độ.")]
    public string? Location { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]
    public string? Address { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập mô tả chi tiết.")]
    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }
}
