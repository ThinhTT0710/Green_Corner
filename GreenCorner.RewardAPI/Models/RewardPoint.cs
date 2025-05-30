using System;
using System.Collections.Generic;

namespace GreenCorner.RewardAPI.Models;

public partial class RewardPoint
{
    public int RewardPointsId { get; set; }

    public string UserId { get; set; } = null!;

    public int TotalPoints { get; set; }
}
