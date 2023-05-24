using System;
using System.Collections.Generic;

namespace communityWeb.Models;

public partial class CommunityRequest
{
    public int Id { get; set; }

    public int? CommunityId { get; set; }

    public int? UserId { get; set; }

    public int? SentByUserId { get; set; }

    public string? Status { get; set; }

    public virtual Community? Community { get; set; }

    public virtual User? SentByUser { get; set; }

    public virtual User? User { get; set; }
}
