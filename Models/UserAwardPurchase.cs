using System;
using System.Collections.Generic;

namespace communityWeb.Models;

public partial class UserAwardPurchase
{
    public int Id { get; set; }

    public int? AwardId { get; set; }

    public int? Quantity { get; set; }

    public double? TotalAmount { get; set; }

    public int? UserId { get; set; }

    public virtual Award? Award { get; set; }

    public virtual User? User { get; set; }
}
