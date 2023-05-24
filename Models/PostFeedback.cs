using System;
using System.Collections.Generic;

namespace communityWeb.Models;

public partial class PostFeedback
{
    public int Id { get; set; }

    public int? PostId { get; set; }

    public int? UserId { get; set; }

    public string? Review { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Post? Post { get; set; }

    public virtual User? User { get; set; }
}
