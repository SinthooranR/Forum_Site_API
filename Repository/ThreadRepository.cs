using Forum_Application_API.Data;
using Forum_Application_API.Interfaces;
using Forum_Application_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum_Application_API.Repository
{
    public class ThreadRepository : IThreadInterface
    {
        private readonly DataContext _context;
        public ThreadRepository(DataContext context)
        {
            _context = context;
        }

        public bool ThreadExists(int threadId)
        {
            return _context.Threads.Any(t => t.Id == threadId);
        }

        public ICollection<ForumThread> GetThreads()
        {
            return _context.Threads.ToList();
        }
        public ForumThread GetThread(int threadId)
        {
            return _context.Threads.Where(t => t.Id == threadId).FirstOrDefault();
        }

        public ICollection<ForumThread> GetThreadsByUser(int userId)
        {
            return _context.Threads.Where(u => u.User.Id == userId).ToList();
        }

        public bool CreateThread(ForumThread thread)
        {
            _context.Add(thread);
            return Save();
        }
        public bool UpdateThread(int userId, ForumThread thread)
        {
            _context.Update(thread);
            return Save();
        }
        public bool DeleteThread(int userId, ForumThread thread)
        {
            _context.Remove(thread);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
