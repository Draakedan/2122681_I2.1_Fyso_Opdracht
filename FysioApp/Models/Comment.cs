using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public DateTime DateMade { get; set; }
        public FysioWorker CommentMadeBy { get; set; }
        public bool VisibleToPatient { get; set; }

        public override string ToString()
        {
            return $"ID: {CommentId}, Text: {CommentText}, DateMade: {DateMade}, Comment made by: {CommentMadeBy}, Visible To Patient: {VisibleToPatient}";
        }
    }
}
