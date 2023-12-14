namespace Forum_Application_API.Dto
{
    //Reduced version of the ForumThread Model for GET
    public class UserForumDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<UserCommentDto> Comments { get; set; }
        public SecureUserDto User { get; set; }
    }
}
