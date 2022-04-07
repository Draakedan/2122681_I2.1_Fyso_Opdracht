using System.Collections.Generic;
using DatabaseHandler.Data;
using DomainModels.Models;
using DomainServices.Repos;

namespace DatabaseHandler.Models
{
    public class CommentRepositroy : IComment
    {
        private readonly FysioDataContext Context;

        public CommentRepositroy(FysioDataContext context) => Context = context;

        public void AddComment(Comment comment, int id)
        {
            Context.Comments.Add(comment);
            Context.SaveChanges();
            new PatientFileRepository(Context).AddComment(comment, id);
        }

        public bool CommentExists(int id) => GetCommentByID(id) != null;

        public List<Comment> GetAllComments()
        {
            List<Comment> commentList = new();
            foreach (Comment c in Context.Comments)
            {
                c.CommentMadeBy = new FysioWorkerRepositroy(Context).GetFysioWorkerByID(c.CommenterID);
                commentList.Add(c);
            }
            return commentList;
        }

        public Comment GetCommentByID(int id)
        {
            foreach (Comment c in Context.Comments)
                if (c.CommentId == id)
                {
                    c.CommentMadeBy = new FysioWorkerRepositroy(Context).GetFysioWorkerByID(c.CommenterID);
                    return c;
                }
            return null;
        }

        public void RemoveAllCommentsForFile(int id)
        {
            PatientFile patientFile = new PatientFileRepository(Context).GetPatientFileByID(id);
            foreach (Comment comment in patientFile.Comments)
                Context.Comments.Remove(comment);
        }
    }
}