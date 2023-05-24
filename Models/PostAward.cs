using System;
using System.Collections.Generic;

namespace communityWeb.Models;

public partial class PostAward
{
    public int Id { get; set; }

    public int? PostId { get; set; }

    public int? AwardId { get; set; }

    public int? UserId { get; set; }

    public DateTime? GivenDate { get; set; }

    public virtual Award? Award { get; set; }

    public virtual Post? Post { get; set; }

    public virtual User? User { get; set; }
}
