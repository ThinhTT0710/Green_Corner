using System;
using System.Collections.Generic;

namespace GreenCorner.EcommerceAPI.Models;

public partial class WishList
{
    public int WishListId { get; set; }

    public string UserId { get; set; }

    public int ProductId { get; set; }

    public Product? Product { get; set; }
}
