using Forum_Application_API.Data;
using Forum_Application_API.Interfaces;
using Forum_Application_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum_Application_API.Repository
{
    public class UserRepository : IUserInterface
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }


        public ICollection<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUser(int id)
        {
            return _context.Users.Where(u => u.Id == id).FirstOrDefault();
        }

        public ICollection<ForumThread> GetThreadsByUser(int userId)
        {
            return _context.Threads.Where(u => u.User.Id == userId).ToList();
        }
        public bool UserExists(int userId)
        {
            return _context.Users.Any(u => u.Id == userId);
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();
        }
        public bool UpdateUser(User user)
        {
            _context.Update(user);
            return Save();
        }
        public bool DeleteUser(User user)
        {
            _context.Remove(user);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
