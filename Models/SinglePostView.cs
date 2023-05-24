namespace communityWeb.Models
{
    public class SinglePostView
    {
        public Post Post { get; set; }
        public List<PostFeedback> Feedbacks { get; set; }
        public string noOfMembers { get; set; }
        public List<Community> userCommunity { get; set; }
    }
}
