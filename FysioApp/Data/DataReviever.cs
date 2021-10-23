using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseHandler.Data;
using DatabaseHandler.Models;
using Newtonsoft.Json.Linq;


namespace FysioAppUX.Data
{
    public class DataReviever
    {
        private readonly IRepository<ActionPlan> ActionPlans;
        private readonly IRepository<Adress> Adresses;
        private readonly IRepository<Comment> Comments;
        private readonly IRepository<FysioWorker> FysioWorkers;
        private readonly IRepository<Patient> Patients;
        private readonly IRepository<PatientFile> PatientFiles;
        private readonly IRepository<TherapySession> TherapySessions;

        private readonly FysioDataContext context;

        public DataReviever(IRepository<ActionPlan> actionPlans, IRepository<Adress> adresses, IRepository<Comment> comments, IRepository<FysioWorker> fysioWorkers, IRepository<Patient> patients, IRepository<PatientFile> patientFiles, IRepository<TherapySession> therapySessions)
        {
            context = new();
            this.ActionPlans = actionPlans;
            this.Adresses = adresses;
            this.Comments = comments;
            this.FysioWorkers = fysioWorkers;
            this.Patients = patients;
            this.PatientFiles = patientFiles;
            this.TherapySessions = therapySessions;

            try
            {
                FillAll();
            }
            catch {
                Console.WriteLine("DatabaseConnection unavailable");
            }

            

        }
        private void FillAll()
        {
            FillActionPlan();
            FillAdress();
            FillFysioWorkers();
            FillComments();
            FillPatients();
            FillTherapySession();
            FillPatientFiles();
        }

        private void FillActionPlan()
        { 
            foreach (ActionPlan a in context.ActionPlans)
                ActionPlans.Add(a);
        }

        private void FillAdress()
        {
            foreach (Adress a in context.Adresses)
                Adresses.Add(a);
        }

        private void FillFysioWorkers()
        {
            foreach (FysioWorker worker in context.FysioWorkers)
                FysioWorkers.Add(worker);
        }

        private void FillComments()
        {
            foreach (Comment c in context.Comments)
            {
                //context.Entry(c)
                //    .Property(com => com.CommentMadeBy);
                Comments.Add(c);
            }
        }

        private void FillPatients()
        {
            foreach (Patient p in context.Patients)
            {
                //context.Entry(p)
                //    .Property(pat => pat.Adress);
                Patients.Add(p);
            }
        }

        private void FillTherapySession()
        {
            foreach (TherapySession ts in context.TherapySessions)
            {
                //context.Entry(ts)
                //    .Property(theraSes => theraSes.SesionDoneBy);
                TherapySessions.Add(ts);
            }
        }

        private void FillPatientFiles()
        {
            foreach (PatientFile file in context.PatientFiles)
            {
                //context.Entry(file)
                //    .Property(f => f.patient);
                //context.Entry(file)
                //    .Property(f => f.intakeDoneBy);
                //context.Entry(file)
                //    .Property(f => f.intakeSuppervisedBy);
                //context.Entry(file)
                //    .Property(f => f.mainTherapist);
                //context.Entry(file)
                //    .Property(f => f.actionPlan);
                context.Entry(file)
                    .Collection(f => f.comments)
                    .Load();
                context.Entry(file)
                    .Collection(f => f.sessions)
                    .Load();
                PatientFiles.Add(file);
            }
        }

    }
}
