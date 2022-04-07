using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DomainModels.Models
{
    public class PatientFile
    {
        [Key]
        public int ID { get; set; }
        public int PatientID { get; set; }
        public Patient Patient { get; set; }
        public int Age { get; set; }
        public string IssueDescription { get; set; }
        public string DiagnoseCode { get; set; }
        public string DiagnoseCodeComment { get; set; }
        public bool IsStudent { get; set; }
        public int IntakeDoneByID { get; set; }
        public FysioWorker IntakeDoneBy { get; set; }
        public int IdintakeSuppervisedBy { get; set; }
        public FysioWorker IntakeSuppervisedBy { get; set; }
        public int IdmainTherapist{ get; set; }
        public FysioWorker MainTherapist { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime FireDate { get; set; }
        public string CommentIDs { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public int IdactionPlan { get; set; }
        public ActionPlan ActionPlan { get; set; }
        public string SessionIDs { get; set; }
        public ICollection<TherapySession> Sessions { get; set; }
    }
}