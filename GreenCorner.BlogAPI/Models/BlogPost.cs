using System;
using System.Collections.Generic;

namespace GreenCorner.BlogAPI.Models;

public partial class BlogPost
{
    public int BlogId { get; set; }

    public string AuthorId { get; set; } = null!;

    public string? Title { get; set; }

    public string? Content { get; set; }

    public string? ThumbnailUrl { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<BlogFavorite> BlogFavorites { get; set; } = new List<BlogFavorite>();

    public virtual ICollection<BlogReport> BlogReports { get; set; } = new List<BlogReport>();
}
