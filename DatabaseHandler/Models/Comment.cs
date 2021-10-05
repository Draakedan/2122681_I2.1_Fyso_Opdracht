using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DatabaseHandler.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public DateTime DateMade { get; set; }
        public int CommenterID { get; set; }
        public FysioWorker CommentMadeBy { get; set; }
        public bool VisibleToPatient { get; set; }
    }
}
