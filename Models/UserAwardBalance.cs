using System;
using System.Collections.Generic;

namespace communityWeb.Models;

public partial class UserAwardBalance
{
    public int Id { get; set; }

    public int? AwardId { get; set; }

    public double? Balance { get; set; }

    public virtual Award? Award { get; set; }
}
