using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DatabaseHandler.Models
{
    public class PatientFile
    {
#nullable enable
        [Key]
        public int ID { get; set; }

        public int patientID { get; set; }

        public Patient patient { get; set; }

        public int age { get; set; }

        public string issueDescription { get; set; }

        public string diagnoseCode { get; set; }

        public string diagnoseCodeComment { get; set; }

        public bool isStudent { get; set; }

        public int intakeDoneByID { get; set; }

        public FysioWorker intakeDoneBy { get; set; }

        public int? IdintakeSuppervisedBy { get; set; }

        public FysioWorker? intakeSuppervisedBy { get; set; }

        public int IdmainTherapist{ get; set; }

        public FysioWorker mainTherapist { get; set; }

        public DateTime registerDate { get; set; }

        public DateTime? fireDate { get; set; }

        public string? CommentIDs { get; set; }

        public ICollection<Comment>? comments { get; set; }

        public int IdactionPlan { get; set; }

        public ActionPlan actionPlan { get; set; }

        public string? sessionIDs { get; set; }

        public ICollection<TherapySession>? sessions { get; set; }




    }
}
