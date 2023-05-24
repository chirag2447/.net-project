namespace communityWeb.Models
{
    public class SearchPostView
    {
        public List<Post> Posts { get; set; }
        public List<Community> Communities { get; set; }
        public List<PostFeedback> Feedbacks { get; set; }
        public List<CommunityMember> Members { get; set; }
        public List<Community> userCommunity { get; set; }
       
    }
}
