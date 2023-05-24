using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace communityWeb.Models;

public partial class User
{
    public int Id { get; set; }
    [DisplayName("FirstName")]
    public string? Fname { get; set; }
    [DisplayName("LastName")]

    public string? Lname { get; set; }

    public string? EmailId { get; set; }

    public string? Password { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? ContactNo { get; set; }

    public bool IsActive { get; set; }

    public string? Gender { get; set; }

    public DateTime? RegisteredDate { get; set; }

    public virtual ICollection<Community> Communities { get; } = new List<Community>();

    public virtual ICollection<CommunityMember> CommunityMembers { get; } = new List<CommunityMember>();

    public virtual ICollection<CommunityRequest> CommunityRequestSentByUsers { get; } = new List<CommunityRequest>();

    public virtual ICollection<CommunityRequest> CommunityRequestUsers { get; } = new List<CommunityRequest>();

    public virtual ICollection<FriendRequest> FriendRequestReceivers { get; } = new List<FriendRequest>();

    public virtual ICollection<FriendRequest> FriendRequestSenders { get; } = new List<FriendRequest>();

    public virtual ICollection<PostAward> PostAwards { get; } = new List<PostAward>();

    public virtual ICollection<PostFeedback> PostFeedbacks { get; } = new List<PostFeedback>();

    public virtual ICollection<Post> Posts { get; } = new List<Post>();

    public virtual ICollection<UserAwardPurchase> UserAwardPurchases { get; } = new List<UserAwardPurchase>();
}
