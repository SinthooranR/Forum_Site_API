namespace Forum_Application_API.Models
{
    public class ThreadOwner
    {
        public int ThreadId { get; set; }

        public int UserId { get; set; }

        public ForumThread ForumThread { get; set; }

        public User User { get; set; }
    }
}
