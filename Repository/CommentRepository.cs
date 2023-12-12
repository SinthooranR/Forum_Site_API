using Forum_Application_API.Data;
using Forum_Application_API.Interfaces;
using Forum_Application_API.Models;

namespace Forum_Application_API.Repository
{
    public class CommentRepository : ICommentInterface
    {
        private readonly DataContext _context;
        public CommentRepository(DataContext context)
        {
            _context = context;
        }

        public bool CommentExists(int commentId)
        {
            return _context.Comments.Any(c => c.Id == commentId);
        }
        public ICollection<Comment> GetComments()
        {
            return _context.Comments.ToList();
        }


        public Comment GetComment(int id)
        {
            return _context.Comments.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Comment> GetCommentsByUser(int userId)
        {
            return _context.Comments.Where(u => u.User.Id == userId).ToList();
        }


        public ICollection<Comment> GetCommentsByThread(int threadId)
        {
            return _context.Comments.Where(u => u.Thread.Id == threadId).ToList();
        }


        public bool CreateComment(Comment comment)
        {
            _context.Comments.Add(comment);
            return Save();
        }

        public bool DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);
            return Save();
        }

        public bool DeleteComments(List<Comment> comments)
        {
            _context.Comments.RemoveRange(comments);
            return Save();
        }
        public bool UpdateComment(Comment comment)
        {
            _context.Comments.Update(comment);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
