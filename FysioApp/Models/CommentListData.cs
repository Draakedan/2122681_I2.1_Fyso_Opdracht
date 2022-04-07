using DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioAppUX.Models
{
    public class CommentListData
    {
        public ICollection<Comment> Comments { get; set; }
        public int DossierID { get; set; }
        public bool IsPatient { get; set; }
        public CommentListData(ICollection<Comment> comments, int dossierId, bool isPatient)
        {
            Comments = comments;
            DossierID = dossierId;
            IsPatient = isPatient;
        }
    }
}
