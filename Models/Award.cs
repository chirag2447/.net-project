using System;
using System.Collections.Generic;

namespace communityWeb.Models;

public partial class Award
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public double? Price { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<PostAward> PostAwards { get; } = new List<PostAward>();

    public virtual ICollection<UserAwardBalance> UserAwardBalances { get; } = new List<UserAwardBalance>();

    public virtual ICollection<UserAwardPurchase> UserAwardPurchases { get; } = new List<UserAwardPurchase>();
}
