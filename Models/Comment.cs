using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum_Application_API.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }


        [ForeignKey("ThreadId")]
        public int ThreadId { get; set; }

        public ForumThread Thread { get; set; }


        [ForeignKey("UserId")]
        public int UserId { get; set; }

        public User User { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

    }
}
