namespace Forum_Application_API.Models
{
    public class CommentOwner
    {
        public int ThreadId { get; set; }

        public int UserId { get; set; }

        public int CommentId { get; set; }
        public Comment Comment { get; set; }
        public ForumThread ForumThread { get; set; }

        public User User { get; set; }
    }
}
