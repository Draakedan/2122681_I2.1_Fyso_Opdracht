using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models
{
    public class PatientFile
    {
        public int ID { get; set; }

        public Patient Patient { get; set; }

        public int Age { get; set; }

        public string IssueDescription { get; set; }

        public string DiagnoseCode { get; set; }

        public bool IsStudent { get; set; }

        public FysioWorker IntakeDoneBy { get; set; }

        public FysioWorker IntakeSuppervisedBy { get; set; }

        public FysioWorker MainTherapist { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime FireDate { get; set; }

        public List<Comment> Comments { get; set; }

        public ActionPlan ActionPlan { get; set; }

        public List<TherapySession> Sessions { get; set; }

        public override string ToString()
        {
            return $"age: {Age}, issueDescription: {IssueDescription}, DiagnoseCode: {DiagnoseCode}, Comments: {PrintComments()}";
        }

        private string PrintComments()
        {
            string s = "";
            foreach (Comment c in Comments)
                s += c.ToString();
            return s;
        }

    }
}
