using System;
using System.Collections.Generic;

namespace GreenCorner.EcommerceAPI.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public string UserId { get; set; } = null!;

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;
}
