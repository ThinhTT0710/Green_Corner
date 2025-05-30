using System;
using System.Collections.Generic;

namespace GreenCorner.BlogAPI.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public string LeaderId { get; set; } = null!;

    public string? Title { get; set; }

    public string? Content { get; set; }

    public DateTime? CreatedAt { get; set; }
}
