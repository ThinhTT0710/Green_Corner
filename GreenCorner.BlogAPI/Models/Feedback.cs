using System;
using System.Collections.Generic;

namespace GreenCorner.BlogAPI.Models;

public partial class Feedback
{
    public int FeedBackId { get; set; }

    public string UserId { get; set; } = null!;

    public string? Title { get; set; }

    public string? Content { get; set; }

    public DateTime? CreatedAt { get; set; }
}
