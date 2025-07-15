using System;
using System.Collections.Generic;

namespace GreenCorner.RewardAPI.Models;

public partial class PointTransaction
{
    public int PointTransactionsId { get; set; }

    public string UserId { get; set; } = null!;

    public string? Type { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int Points { get; set; }

    public int? CleanEventId { get; set; }
}
