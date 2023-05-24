namespace communityWeb.Models
{
    public class communityDetailView
    {
            public Community Community { get; set; }
            public List<CommunityMember> Members { get; set; }
            public bool IsMember { get; set; }
            public List<Post> Posts { get; set; }
    }
}
