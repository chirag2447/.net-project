using System;
using System.Collections.Generic;

namespace communityWeb.Models;

public partial class Community
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? OwnerId { get; set; }

    public string? Logo { get; set; }

    public bool IsApproved { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<CommunityMember> CommunityMembers { get; } = new List<CommunityMember>();

    public virtual ICollection<CommunityRequest> CommunityRequests { get; } = new List<CommunityRequest>();

    public virtual User? Owner { get; set; }

    public virtual ICollection<Post> Posts { get; } = new List<Post>();
}
