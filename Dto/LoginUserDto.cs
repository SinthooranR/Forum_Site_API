using System.ComponentModel.DataAnnotations;

namespace Forum_Application_API.Dto
{
    public class LoginUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
