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

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Patient patient { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public int age { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string issueDescription { get; set; }


        public string diagnoseCode { get; set; }

        public string diagnoseCodeComment { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public bool isStudent { get; set; }

        public int intakeDoneByID { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public FysioWorker intakeDoneBy { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public int? IdintakeSuppervisedBy { get; set; }

        public FysioWorker? intakeSuppervisedBy { get; set; }

        public int IdmainTherapist{ get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public FysioWorker mainTherapist { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public DateTime registerDate { get; set; }

        public DateTime? fireDate { get; set; }

        public string? CommentIDs { get; set; }

        public ICollection<Comment>? comments { get; set; }

        public int IdactionPlan { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ActionPlan actionPlan { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public string? sessionIDs { get; set; }

        public ICollection<TherapySession>? sessions { get; set; }

        public string getShortFireDateString()
        {
            if (fireDate.HasValue)
            {
                DateTime date = fireDate ?? default(DateTime);
                return date.ToShortDateString();
            }
            return "N/A";

        }

        public string getIntakeSuppervisorName()
        {
            if (intakeSuppervisedBy == null)
                return "Er was geen toezicht nodig tijdens intake";
            else
                return intakeSuppervisedBy.Name;
        }


    }
}
