using DatabaseHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioAppUX.Models
{
    public class CommentListData
    {
        public ICollection<Comment> comments { get; set; }
        public int dossierID { get; set; }
        public CommentListData()
        { }
    }
}
