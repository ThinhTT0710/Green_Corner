

using System.ComponentModel.DataAnnotations;

namespace GreenCorner.MVC.Models;

public partial class TrashEventDTO
{
    public int TrashReportId { get; set; }

    [Required(ErrorMessage = "Người gửi báo cáo không được để trống.")]
    public string UserId { get; set; } = null!;
    [Required(ErrorMessage = "Vui lòng cung cấp tọa độ.")]
    public string? Location { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]
    public string? Address { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập mô tả chi tiết.")]
    public string? Description { get; set; }

    [Url(ErrorMessage = "Đường dẫn hình ảnh không hợp lệ.")]
    public string? ImageUrl { get; set; }

    [StringLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự.")]
    public string? Status { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? CreatedAt { get; set; }
}
