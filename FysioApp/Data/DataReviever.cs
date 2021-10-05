using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseHandler.Data;
using FysioApp.Models;
using Newtonsoft.Json.Linq;


namespace FysioApp.Data
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

        private readonly DataRepository DataRepo;

        public DataReviever(IRepository<ActionPlan> actionPlans, IRepository<Adress> adresses, IRepository<Comment> comments, IRepository<FysioWorker> fysioWorkers, IRepository<Patient> patients, IRepository<PatientFile> patientFiles, IRepository<TherapySession> therapySessions)
        {
            DataRepo = new();
            this.ActionPlans = actionPlans;
            this.Adresses = adresses;
            this.Comments = comments;
            this.FysioWorkers = fysioWorkers;
            this.Patients = patients;
            this.PatientFiles = patientFiles;
            this.TherapySessions = therapySessions;

            FillAll();

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
            JArray array = DataRepo.GetActionPlans();
            ActionPlan action;
            foreach (JObject o in array)
            {
                action = new()
                {
                    ActionID = (int)o["ActionID"],
                    SessionsPerWeek = (int)o["SessionsPerWeek"],
                    TimePerSession = (int)o["TimePerSession"]
                };
                ActionPlans.Add(action);
            }
        }

        private void FillAdress()
        {
            JArray array = DataRepo.GetAdresses();
            Adress adress;
            foreach (JObject o in array)
            {
                adress = new()
                {
                    AdressID = (int)o["AdressID"],
                    Country = (string)o["Country"],
                    City = (string)o["City"],
                    PostalCode = (string)o["PostalCode"],
                    Street = (string)o["Street"],
                    HouseNumber = (string)o["HouseNumber"]
                };
                Adresses.Add(adress);
            }
        }

        private void FillFysioWorkers()
        {
            JArray array = DataRepo.GetFysioWorkers();
            FysioWorker worker;
            foreach (JObject o in array)
            {
                worker = new()
                {
                    FysioWorkerID = (int)o["FysioWorkerID"],
                    Name = (string)o["Name"],
                    Email = (string)o["Email"],
                    PhoneNumber = (string)o["PhoneNumber"],
                    WorkerNumber = (string)o["WorkerNumber"],
                    BIGNumber = (string)o["BIGNumber"],
                    StudentNumber = (string)o["StudentNumber"],
                    IsStudent = (bool)o["IsStudent"]
                };
                FysioWorkers.Add(worker);
            }
        }

        private void FillComments()
        {
            JArray array = DataRepo.GetComments();
            Comment comment;
            foreach (JObject o in array)
            {
                comment = new()
                {
                    CommentId = (int)o["CommentId"],
                    CommentText = (string)o["CommentText"],
                    DateMade = (DateTime)o["DateMade"],
                    CommentMadeBy = FysioWorkers.GetItemByID((int)o["CommenterID"]),
                    VisibleToPatient = (bool)o["VisibleToPatient"]
                };
                Comments.Add(comment);
            }
        }

        private void FillPatients()
        {
            JArray array = DataRepo.GetPatients();
            Patient patient;
            foreach (JObject o in array)
            {
                patient = new()
                {
                    PatientID = (int)o["PatientID"],
                    EnsuranceCompany = (string)o["EnsuranceCompany"],
                    Email = (string)o["Email"],
                    PhoneNumber = (string)o["PhoneNumber"],
                    Adress = Adresses.GetItemByID((int)o["AdressID"]),
                    StudentNumber = (string)o["StudentNumber"],
                    WorkerNumber = (string)o["WorkerNumber"],
                    Name = (string)o["Name"],
                    PatientNumber = (string)o["PatientNumber"],
                    IsMale = (bool)o["IsMale"],
                    Age = (int)o["Age"],
                    Birthdate = (DateTime)o["Birthdate"],
                    IsStudent = (bool)o["IsStudent"]
                };
                Patients.Add(patient);
            }
        }

        private void FillTherapySession()
        {
            JArray array = DataRepo.GetTherapySessions();
            TherapySession therapySession;
            foreach (JObject o in array)
            {
                therapySession = new()
                {
                    Id = (int)o["TherapySessionId"],
                    Type = (int)o["Type"],
                    Description = (string)o["Description"],
                    Location = (string)o["Location"],
                    Specials = (string)o["Specials"],
                    SesionDoneBy = FysioWorkers.GetItemByID((int)o["SessionDoneByID"]),
                    SessionTime = (DateTime)o["SessionTime"]
                };
                TherapySessions.Add(therapySession);
            }
        }

        private void FillPatientFiles()
        {
            JArray array = DataRepo.GetPatientFiles();
            PatientFile file;
            foreach (JObject o in array)
            {
                file = new()
                {
                    ID = (int)o["ID"],
                    Patient = Patients.GetItemByID((int)o["patientID"]),
                    Age = (int)o["age"],
                    IssueDescription = (string)o["issueDescription"],
                    DiagnoseCode = (string)o["diagnoseCode"],
                    IsStudent = (bool)o["isStudent"],
                    IntakeDoneBy = FysioWorkers.GetItemByID((int)o["intakeDoneByID"]),
                    IntakeSuppervisedBy = FysioWorkers.GetItemByID((int)o["IdintakeSuppervisedBy"]),
                    MainTherapist = FysioWorkers.GetItemByID((int)o["IdmainTherapist"]),
                    RegisterDate = (DateTime)o["registerDate"],
                    Comments = GetComments((string)o["CommentIDs"]),
                    ActionPlan = ActionPlans.GetItemByID((int)o["IdactionPlan"]),
                    Sessions = GetSessions((string)o["sessionIDs"])
                };

                try
                {
                    file.FireDate = (DateTime)o["fireDate"];
                }
                catch {
                }
                PatientFiles.Add(file);
            }
        }

        private List<Comment> GetComments(string s)
        {
            List<Comment> comments = new();

            string[] list = s.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in list)
                comments.Add(Comments.GetItemByID(int.Parse(item)));

            return comments;
        }

        private List<TherapySession> GetSessions(string s)
        {
            List<TherapySession> sessions = new();

            string[] list = s.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in list)
                sessions.Add(TherapySessions.GetItemByID(int.Parse(item)));

            return sessions;
        }

    }
}
