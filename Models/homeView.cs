namespace communityWeb.Models
{
    public class homeView
    {
        public List<Post> Posts { get; set; }
        public List<Community> Communities { get; set; }
        public List<Community> Community { get; set; }
        public List<FriendRequest> FriendRequests { get; set; }
    }
}
