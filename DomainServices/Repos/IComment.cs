using System.Collections.Generic;
using DomainModels.Models;

namespace DomainServices.Repos
{
    public interface IComment
    {
        void AddComment(Comment comment, int id);
        bool CommentExists(int id);
        List<Comment> GetAllComments();
        Comment GetCommentByID(int id);
        void RemoveAllCommentsForFile(int id);
    }
}