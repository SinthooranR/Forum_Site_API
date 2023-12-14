namespace Forum_Application_API.Dto
{

    //Reduces Info shown on return
    public class SecureUserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<UserForumDto> Threads { get; set; }
        public ICollection<UserCommentDto> Comments { get; set; }
    }
}
