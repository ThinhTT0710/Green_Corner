using System;
using System.Collections.Generic;

namespace GreenCorner.BlogAPI.Models;

public partial class BlogReport
{
    public int BlogReportId { get; set; }

    public int BlogId { get; set; }

    public string? Reason { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual BlogPost Blog { get; set; } = null!;
}
