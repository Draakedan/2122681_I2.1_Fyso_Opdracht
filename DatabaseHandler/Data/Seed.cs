using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DomainModels.Models;

namespace DatabaseHandler.Data
{
    class Seed
    {
        private readonly FysioDataContext Context;
        private ActionPlan ActionPlan;
        private Adress Adress;
        private FysioWorker Worker;
        private FysioWorker Student;
        private Comment Comment;
        private Patient Patient;
        private TherapySession TherapySession;
        private TherapySession TherapySession2;

        public Seed(FysioDataContext context)
        {
            Context = context;
            SeedActionPlan();
            SeedAddresses();
            SeedFysioWorkers();
            SeedComments();
            SeedPatients();
            SeedTherapySessions();
            SeedPatientFiles();
        }

        private void SeedActionPlan()
        {
            ActionPlan = new()
            {
                SessionsPerWeek = 3,
                TimePerSession = 2
            };
            Context.ActionPlans.Add(ActionPlan);

            ActionPlan actionPlan2 = new()
            {
                SessionsPerWeek = 1,
                TimePerSession = 2
            };
            Context.ActionPlans.Add(actionPlan2);

            ActionPlan actionPlan3 = new()
            {
                SessionsPerWeek = 3,
                TimePerSession = 1
            };
            Context.ActionPlans.Add(actionPlan3);

            Context.SaveChanges();
        }

        public void SeedAddresses()
        {
            Adress = new()
            {
                Country = "Nederland",
                City = "Breda",
                PostalCode = "4818 AJ",
                Street = "Lovensdijkstraat",
                HouseNumber = "61"
            };
            Context.Adresses.Add(Adress);

            Adress adress1 = new()
            {
                Country = "Nederland",
                City = "Breda",
                PostalCode = "4818 CR",
                Street = "Hogeschoollaan",
                HouseNumber = "1"
            };
            Context.Adresses.Add(adress1);

            Adress adress2 = new()
            {
                Country = "Nederland",
                City = "Roosendaal",
                PostalCode = "4701 BS",
                Street = "Mill Hillplein",
                HouseNumber = "1"
            };
            Context.Adresses.Add(adress2);

            Context.SaveChanges();
        }

        public void SeedComments()
        {
            Comment = new()
            {
                CommentText = "Knie is opgezwollen maar kan nog wel staan een lopen, echter is dit geen goed idee, omdat de knie moet rusten.",
                DateMade = DateTime.Today,
                CommenterID = Student.FysioWorkerID,
                CommentMadeBy = Student,
                VisibleToPatient = true
            };
            Context.Comments.Add(Comment);

            Comment comment2 = new()
            {
                CommentText = "What doet deze man rondlopen met een gezwollen en waarschijnlijk ontstoken knie",
                DateMade = DateTime.Today.AddDays(3),
                CommentMadeBy = Worker,
                CommenterID = Worker.FysioWorkerID,
                VisibleToPatient = false
            };
            Context.Comments.Add(comment2);

            Context.SaveChanges();
        }

        public void SeedFysioWorkers()
        {
            Worker = new()
            {
                Name = "Rudolf",
                Email = "IAmNotThatRudolf@RedNose.com",
                PhoneNumber = "+2990112358",
                WorkerNumber = "012343",
                BIGNumber = "0012345678",
                IsStudent = false
            };
            Context.FysioWorkers.Add(Worker);

            FysioWorker worker2 = new()
            {
                Name = "Hank",
                Email = "Hankerchief@RedNose.com",
                PhoneNumber = "+3168656866",
                WorkerNumber = "3898798748",
                BIGNumber = "1234567890",
                IsStudent = false
            };
            Context.FysioWorkers.Add(worker2);

            Student = new()
            {
                Name = "Pino",
                Email = "Pinoccio@RealBoi.com",
                PhoneNumber = "+310118999",
                StudentNumber = "881999119",
                IsStudent = true

            };
            Context.FysioWorkers.Add(Student);

            FysioWorker student2 = new()
            {
                Name = "Bart",
                Email = "BartSmidje@Roestig.com",
                PhoneNumber = "+31005556600",
                StudentNumber = "458979756",
                IsStudent = true
            };
            Context.FysioWorkers.Add(student2);

            Context.SaveChanges();
        }

        public void SeedPatients()
        {
            Patient = new()
            {
                EnsuranceCompany = "Insured",
                Email = "Ahmekie@avans.nl",
                PhoneNumber = "+310606060606",
                StudentNumber = "2122693",
                Name = "Ahfieds",
                AdressID = Adress.AdressID,
                Adress = Adress,
                PatientNumber = "000555333111",
                IsMale = true,
                Age = 19,
                Birthdate = DateTime.Now.AddYears(-19).AddDays(-5).AddMonths(-3),
                IsStudent = true
            };
            Context.Patients.Add(Patient);

            Patient patient2 = new()
            {
                EnsuranceCompany = "Interpolis",
                Email = "Hvoedts@avans.nl",
                PhoneNumber = "+31454647657",
                StudentNumber = "1234567123",
                Adress = Adress,
                AdressID = Adress.AdressID,
                Name = "Helga",
                PatientNumber = "34357657564",
                IsMale = false,
                Age = 22,
                Birthdate = DateTime.Now.AddYears(-22).AddDays(-20).AddMonths(-1),
                IsStudent = true
            };
            Context.Patients.Add(patient2);

            Patient patient3 = new()
            {
                EnsuranceCompany = "Interpolis",
                Email = "Rugdeklac@avans.nl",
                PhoneNumber = "+315454767658",
                WorkerNumber = "4654676487845",
                Adress = Adress,
                AdressID = Adress.AdressID,
                Name = "Renie",
                PatientNumber = "3214334567",
                IsMale = true,
                Age = 45,
                Birthdate = DateTime.Now.AddYears(-45).AddDays(-17).AddMonths(-6),
                IsStudent = false
            };
            Context.Patients.Add(patient3);

            Context.SaveChanges();
        }

        public void SeedTherapySessions()
        {
            TherapySession = new()
            {
                Type = "1003",
                Description = "Tijdens deze sessie wordt er aan de benen gewerkt",
                IsPractiseRoom = true,
                SessionDoneByID = Worker.FysioWorkerID,
                SesionDoneBy = Worker,
                SessionStartTime = DateTime.Now.AddDays(10),
                SessionEndTime = DateTime.Now.AddDays(10).AddHours(1),
                CreationDate = DateTime.Now
            };
            Context.TherapySessions.Add(TherapySession);

            TherapySession2 = new()
            {
                Type = "1003",
                Description = "We gaan in deze sesie verder aan de benen",
                IsPractiseRoom = false,
                SesionDoneBy = Student,
                SessionDoneByID = Student.FysioWorkerID,
                SessionStartTime = DateTime.Now.AddDays(13),
                SessionEndTime = DateTime.Now.AddDays(13).AddHours(1),
                CreationDate = DateTime.Now.AddDays(-2)
            };
            Context.TherapySessions.Add(TherapySession2);

            Context.SaveChanges();
        }

        public void SeedPatientFiles()
        {
            List<TherapySession> sessions = new();
            foreach (TherapySession ts in Context.TherapySessions)
            {
                sessions.Add(ts);
            }
            PatientFile file = new()
            {
                PatientID = Patient.PatientID,
                Patient = Patient,
                Age = Patient.Age,
                IssueDescription = "Patient heeft last van zijn been",
                DiagnoseCode = "1111",
                DiagnoseCodeComment = "bla bla bla....",
                IsStudent = Patient.IsStudent,
                IntakeDoneBy = Student,
                IntakeDoneByID = Student.FysioWorkerID,
                IntakeSuppervisedBy = Worker,
                IdintakeSuppervisedBy = Worker.FysioWorkerID,
                IdmainTherapist = Worker.FysioWorkerID,
                MainTherapist = Worker,
                RegisterDate = DateTime.Now.AddYears(-1),
                CommentIDs = ConvertToString(new List<int>() { Comment.CommentId}),
                Comments = new List<Comment> { Comment },
                IdactionPlan = ActionPlan.ActionID,
                ActionPlan = ActionPlan,
                Sessions = sessions,
                SessionIDs = ConvertToString(SeedSessionIDs(sessions))
            };
            Context.PatientFiles.Add(file);

            Context.SaveChanges();
        }

        private static List<int> SeedSessionIDs(List<TherapySession> sessions)
        {
            List<int> ints = new();
            foreach (TherapySession ts in sessions)
            {
                ints.Add(ts.Id);
            }
            return ints;
        }

        private static string ConvertToString(List<int> list)
        {
            string s = "";
            foreach (int i in list)
            {
                s += $"{i} ";
            }

            return s;
        }
    }
}
