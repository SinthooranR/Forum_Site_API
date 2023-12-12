using Forum_Application_API.Models;

namespace Forum_Application_API.Dto
{
    public class ThreadDto {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //public ICollection<Comment> Comments { get; set; }
    }
}
