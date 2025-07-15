using System.ComponentModel.DataAnnotations;

namespace GreenCorner.MVC.Models
{
    public class VoucherDTO : IValidatableObject
    {
        public int VoucherId { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Điểm yêu cầu không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Điểm yêu cầu phải từ 0 trở lên")]
        public int? PointsRequired { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày bắt đầu")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Ngày bắt đầu không được để trống")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày hết hạn")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Ngày hết hạn không được để trống")]
        public DateTime? ExpirationDate { get; set; }

        [Required(ErrorMessage = "Mã khuyến mãi không được để trống")]
        [StringLength(50, ErrorMessage = "Mã khuyến mãi không được vượt quá 50 ký tự")]
        public string? Code { get; set; }

        // Kiểm tra StartDate <= ExpirationDate
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate > ExpirationDate)
            {
                yield return new ValidationResult(
                    "Ngày bắt đầu không được lớn hơn ngày hết hạn.",
                    new[] { nameof(StartDate) }
                );
            }
        }
    }
}
