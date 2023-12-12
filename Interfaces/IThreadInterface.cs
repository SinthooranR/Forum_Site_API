using Forum_Application_API.Models;

namespace Forum_Application_API.Interfaces
{
    public interface IThreadInterface
    {
        ICollection<ForumThread> GetThreads();
        ForumThread GetThread(int threadId);

        ICollection<ForumThread> GetThreadsByUser(int userId);
        bool ThreadExists(int threadId);

        //POST
        bool CreateThread(ForumThread thread);
        bool UpdateThread(int userId, ForumThread thread);
        bool DeleteThread(int userId, ForumThread thread);

        //bool DeleteThread(List<ForumThread> threads);
        bool Save();
    }
}
