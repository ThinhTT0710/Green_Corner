using System;
using System.Collections.Generic;

namespace GreenCorner.RewardAPI.Models;

public partial class UserVoucherRedemption
{
    public int UserVoucherId { get; set; }

    public int VoucherId { get; set; }

    public string UserId { get; set; } = null!;

    public DateTime? RedeemedAt { get; set; }

    public bool IsUsed { get; set; }

    public virtual Voucher Voucher { get; set; } = null!;
}
