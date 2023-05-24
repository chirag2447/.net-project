using System;
using System.Collections.Generic;

namespace communityWeb.Models;

public partial class Post
{
    public int Id { get; set; }

    public int? CommunityId { get; set; }

    public int? UserId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Type { get; set; }

    public string? FileName { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Community? Community { get; set; }

    public virtual ICollection<PostAward> PostAwards { get; } = new List<PostAward>();

    public virtual ICollection<PostFeedback> PostFeedbacks { get; } = new List<PostFeedback>();

    public virtual User? User { get; set; }
}
