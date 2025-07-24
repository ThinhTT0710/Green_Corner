using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenCorner.MVC.Models
{
    public class OrderDTO
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage = "UserId không được để trống.")]
        public string UserId { get; set; } = null!;

        [Required(ErrorMessage = "Trạng thái đơn hàng là bắt buộc.")]
        public string Status { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền phải là số không âm.")]
        public decimal TotalMoney { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán.")]
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "Họ tên người nhận không được để trống.")]
        [StringLength(255, ErrorMessage = "Họ tên không được vượt quá 255 ký tự.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Địa chỉ giao hàng không được để trống.")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ghi chú là bắt buộc.")]
        [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự.")]
        public string Note { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<OrderDetailDTO>? OrderDetailsDTO { get; set; }
    }
}
