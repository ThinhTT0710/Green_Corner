using System;
using System.Collections.Generic;

namespace GreenCorner.RewardAPI.Models;

public partial class Voucher
{
    public int VoucherId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? PointsRequired { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public string? Code { get; set; }

    public virtual ICollection<UserVoucherRedemption> UserVoucherRedemptions { get; set; } = new List<UserVoucherRedemption>();
}
