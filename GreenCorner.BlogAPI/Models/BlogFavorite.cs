using System;
using System.Collections.Generic;

namespace GreenCorner.BlogAPI.Models;

public partial class BlogFavorite
{
    public int BlogFavoriteId { get; set; }

    public int BlogId { get; set; }

    public string UserId { get; set; } = null!;

    public virtual BlogPost Blog { get; set; } = null!;
}
