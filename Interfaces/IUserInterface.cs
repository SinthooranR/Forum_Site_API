using Forum_Application_API.Models;

namespace Forum_Application_API.Interfaces
{
    public interface IUserInterface
    {
        ICollection<User> GetUsers();
        User GetUser(int id);
        bool UserExists(int userId);
        ICollection<ForumThread> GetThreadsByUser(int userId);
        //FOR POST REQS
        bool CreateUser(User user);

        bool UpdateUser(User user);

        bool DeleteUser(User user);
        bool Save();
    }
}
