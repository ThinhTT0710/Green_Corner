using System.ComponentModel.DataAnnotations;

namespace GreenCorner.MVC.Models
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
        [StringLength(255, ErrorMessage = "Tên sản phẩm không được vượt quá 255 ký tự.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc.")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc.")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0.")]
        public int Quantity { get; set; }

        [Url(ErrorMessage = "Đường dẫn hình ảnh không hợp lệ.")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Mô tả là bắt buộc.")]
        public string? Description { get; set; }

        [Range(0, 100, ErrorMessage = "Giảm giá phải nằm trong khoảng từ 0 đến 100.")]
        public int? Discount { get; set; }

        [StringLength(255, ErrorMessage = "Danh mục không được vượt quá 255 ký tự.")]
        public string? Category { get; set; }

        [StringLength(255, ErrorMessage = "Thương hiệu không được vượt quá 255 ký tự.")]
        public string? Brand { get; set; }

        [StringLength(255, ErrorMessage = "Xuất xứ không được vượt quá 255 ký tự.")]
        public string? Origin { get; set; }

        public bool? IsDeleted { get; set; } = false;

        [DataType(DataType.DateTime)]
        public DateTime? CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }
    }
}
