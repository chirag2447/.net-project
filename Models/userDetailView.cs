namespace communityWeb.Models
{
    public class userDetailView
    {
        public User User { get; set; }
        public List<CommunityMember> Members { get; set; }
        public bool IsMember { get; set; }
        public List<Post> Posts { get; set; }
        public FriendRequest Friend { get; set; }
        public int noOfFriends { get; set; }
    }
}
