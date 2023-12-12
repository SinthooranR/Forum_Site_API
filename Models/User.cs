using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Forum_Application_API.Models
{
    public class User : IdentityUser<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
        public ICollection<ForumThread> Threads { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
