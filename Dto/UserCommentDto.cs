namespace Forum_Application_API.Dto
{
    //Reduced version of the Comment Model for GET
    public class UserCommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }

        public int ThreadId { get; set; }
        public int UserId { get; set; }
        public SecureUserDto User { get; set; }
    }
}
