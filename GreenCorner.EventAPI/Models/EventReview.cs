using System;
using System.Collections.Generic;

namespace GreenCorner.EventAPI.Models;

public partial class EventReview
{
    public int EventReviewId { get; set; }

    public int CleanEventId { get; set; }

    public string UserId { get; set; } = null!;

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual CleanupEvent CleanEvent { get; set; } = null!;
}
