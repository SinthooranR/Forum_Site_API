using Forum_Application_API.Models;

namespace Forum_Application_API.Interfaces
{
    public interface ICommentInterface
    {
        ICollection<Comment> GetComments();

        Comment GetComment(int id);
        bool CommentExists(int commentId);
        ICollection<Comment> GetCommentsByUser(int userId);
        ICollection<Comment> GetCommentsByThread(int threadId);
        bool CreateComment(Comment comment);
        bool UpdateComment(Comment comment);
        bool DeleteComment(Comment comment);

        bool DeleteComments(List<Comment> comments);

        bool Save();
    }
}
