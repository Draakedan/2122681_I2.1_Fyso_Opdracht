using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DatabaseHandler.Models;

namespace DatabaseHandler.Data
{
    class Seed
    {
        FysioDataContext context = new();
        ActionPlan actionPlan;
        Adress adress;
        FysioWorker worker;
        FysioWorker student;
        Comment comment;
        Patient patient;
        TherapySession therapySession;
        TherapySession therapySession2;

        public Seed()
        {
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
            actionPlan = new()
            {
                SessionsPerWeek = 3,
                TimePerSession = 2
            };
            context.ActionPlans.Add(actionPlan);

            ActionPlan actionPlan2 = new ActionPlan()
            {
                SessionsPerWeek = 1,
                TimePerSession = 2
            };
            context.ActionPlans.Add(actionPlan2);

            ActionPlan actionPlan3 = new()
            {
                SessionsPerWeek = 3,
                TimePerSession = 1
            };
            context.ActionPlans.Add(actionPlan3);

            context.SaveChanges();

        }

        public void SeedAddresses()
        {
            adress = new()
            {
                Country = "Nederland",
                City = "Breda",
                PostalCode = "4818 AJ",
                Street = "Lovensdijkstraat",
                HouseNumber = "61"
            };
            context.Adresses.Add(adress);

            Adress adress1 = new()
            {
                Country = "Nederland",
                City = "Breda",
                PostalCode = "4818 CR",
                Street = "Hogeschoollaan",
                HouseNumber = "1"
            };
            context.Adresses.Add(adress1);

            Adress adress2 = new()
            {
                Country = "Nederland",
                City = "Roosendaal",
                PostalCode = "4701 BS",
                Street = "Mill Hillplein",
                HouseNumber = "1"
            };
            context.Adresses.Add(adress2);

            context.SaveChanges();
        }

        public void SeedComments()
        {
            comment = new()
            {
                CommentText = "Knie is opgezwollen maar kan nog wel staan een lopen, echter is dit geen goed idee, omdat de knie moet rusten.",
                DateMade = DateTime.Today,
                CommenterID = student.FysioWorkerID,
                CommentMadeBy = student,
                VisibleToPatient = true
            };
            context.Comments.Add(comment);

            Comment comment2 = new()
            {
                CommentText = "What doet deze man rondlopen met een gezwollen en waarschijnlijk ontstoken knie",
                DateMade = DateTime.Today.AddDays(3),
                CommentMadeBy = worker,
                CommenterID = worker.FysioWorkerID,
                VisibleToPatient = false
            };
            context.Comments.Add(comment2);

            context.SaveChanges();
        }

        public void SeedFysioWorkers()
        {
            worker = new()
            {
                Name = "Rudolf",
                Email = "IAmNotThatRudolf@RedNose.com",
                PhoneNumber = "+2990112358",
                WorkerNumber = "012343",
                BIGNumber = "0012345678",
                IsStudent = false
            };
            context.FysioWorkers.Add(worker);

            FysioWorker worker2 = new()
            {
                Name = "Hank",
                Email = "Hankerchief@RedNose.com",
                PhoneNumber = "+3168656866",
                WorkerNumber = "3898798748",
                BIGNumber = "1234567890",
                IsStudent = false
            };
            context.FysioWorkers.Add(worker2);

            student = new()
            {
                Name = "Pino",
                Email = "Pinoccio@RealBoi.com",
                PhoneNumber = "+310118999",
                StudentNumber = "881999119",
                IsStudent = true

            };
            context.FysioWorkers.Add(student);

            FysioWorker student2 = new()
            {
                Name = "Bart",
                Email = "BartSmidje@Roestig.com",
                PhoneNumber = "+31005556600",
                StudentNumber = "458979756",
                IsStudent = true
            };
            context.FysioWorkers.Add(student2);

            context.SaveChanges();
        }

        public void SeedPatients()
        {
            patient = new()
            {
                EnsuranceCompany = "Insured",
                Email = "Ahmekie@avans.nl",
                PhoneNumber = "+310606060606",
                StudentNumber = "2122693",
                Name = "Ahfieds",
                AdressID = adress.AdressID,
                Adress = adress,
                PatientNumber = "000555333111",
                IsMale = true,
                Age = 19,
                Birthdate = DateTime.Now.AddYears(-19).AddDays(-5).AddMonths(-3),
                IsStudent = true
            };
            context.Patients.Add(patient);

            Patient patient2 = new()
            {
                EnsuranceCompany = "Interpolis",
                Email = "Hvoedts@avans.nl",
                PhoneNumber = "+31454647657",
                StudentNumber = "1234567123",
                Adress = adress,
                AdressID = adress.AdressID,
                Name = "Helga",
                PatientNumber = "34357657564",
                IsMale = false,
                Age = 22,
                Birthdate = DateTime.Now.AddYears(-22).AddDays(-20).AddMonths(-1),
                IsStudent = true
            };
            context.Patients.Add(patient2);

            Patient patient3 = new()
            {
                EnsuranceCompany = "Interpolis",
                Email = "Rugdeklac@avans.nl",
                PhoneNumber = "+315454767658",
                WorkerNumber = "4654676487845",
                Adress = adress,
                AdressID = adress.AdressID,
                Name = "Renie",
                PatientNumber = "3214334567",
                IsMale = true,
                Age = 45,
                Birthdate = DateTime.Now.AddYears(-45).AddDays(-17).AddMonths(-6),
                IsStudent = false
            };
            context.Patients.Add(patient3);

            context.SaveChanges();
        }

        public void SeedTherapySessions()
        {
            therapySession = new()
            {
                Type = "1003",
                Description = "Tijdens deze sessie wordt er aan de benen gewerkt",
                IsPractiseRoom = true,
                SessionDoneByID = worker.FysioWorkerID,
                SesionDoneBy = worker,
                SessionStartTime = DateTime.Now.AddDays(10),
                SessionEndTime = DateTime.Now.AddDays(10).AddHours(1),
                CreationDate = DateTime.Now
            };
            context.TherapySessions.Add(therapySession);

            therapySession2 = new()
            {
                Type = "1003",
                Description = "We gaan in deze sesie verder aan de benen",
                IsPractiseRoom = false,
                SesionDoneBy = student,
                SessionDoneByID = student.FysioWorkerID,
                SessionStartTime = DateTime.Now.AddDays(13),
                SessionEndTime = DateTime.Now.AddDays(13).AddHours(1),
                CreationDate = DateTime.Now.AddDays(-2)
            };
            context.TherapySessions.Add(therapySession2);

            context.SaveChanges();
        }

        public void SeedPatientFiles()
        {
            List<TherapySession> sessions = new();
            foreach (TherapySession ts in context.TherapySessions)
            {
                sessions.Add(ts);
            }
            PatientFile file = new()
            {
                patientID = patient.PatientID,
                patient = patient,
                age = patient.Age,
                issueDescription = "Patient heeft last van zijn been",
                diagnoseCode = "1111",
                diagnoseCodeComment = "bla bla bla....",
                isStudent = patient.IsStudent,
                intakeDoneBy = student,
                intakeDoneByID = student.FysioWorkerID,
                intakeSuppervisedBy = worker,
                IdintakeSuppervisedBy = worker.FysioWorkerID,
                IdmainTherapist = worker.FysioWorkerID,
                mainTherapist = worker,
                registerDate = DateTime.Now.AddYears(-1),
                CommentIDs = ConvertToString(new List<int>() { comment.CommentId}),
                comments = new List<Comment> { comment },
                IdactionPlan = actionPlan.ActionID,
                actionPlan = actionPlan,
                sessions = sessions,
                sessionIDs = ConvertToString(seedSessionIDs(sessions))
            };
            context.PatientFiles.Add(file);

            context.SaveChanges();
        }

        private List<int> seedSessionIDs(List<TherapySession> sessions)
        {
            List<int> ints = new();
            foreach (TherapySession ts in sessions)
            {
                ints.Add(ts.Id);
            }
            return ints;
        }

        private string ConvertToString(List<int> list)
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
