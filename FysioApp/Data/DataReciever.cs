using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseHandler.Data;
using DatabaseHandler.Models;
using Newtonsoft.Json.Linq;


namespace FysioAppUX.Data
{
    public class DataReciever
    {

        private readonly FysioDataContext context;

        public DataReciever()
        {
            context = new();
        }

        public ICollection<ActionPlan> GetAllActionPlans()
        {

            List<ActionPlan> coll = new();
            foreach (ActionPlan a in context.ActionPlans)
                coll.Add(a);
            return coll;
        }

        public ActionPlan GetOneActionPlan(int id)
        {
            foreach (ActionPlan p in context.ActionPlans)
                if (p.ActionID == id)
                    return p;
            return null;
        }

        public void UpdateActionPlan(ActionPlan plan, PatientFile file)
        {
            context.ActionPlans.Update(plan);
            context.SaveChanges();
            UpdatePatientFile(file);
        }

        public void AddActionPlan(ActionPlan plan)
        {
            context.ActionPlans.Add(plan);
            context.SaveChanges();
        }

        public ICollection<Adress> GetAllAdresses()
        {

            List<Adress> coll = new();
            foreach (Adress a in context.Adresses)
                coll.Add(a);
            return coll;
        }

        public Adress GetOneAdress(int id)
        {
            foreach (Adress a in context.Adresses)
                if (a.AdressID == id)
                    return a;
            return null;
        }

        public void AddAdress(Adress adress)
        {
            context.Adresses.Add(adress);
            context.SaveChanges();
        }

        public void UpdateAdress(Adress adress, int patientId)
        {
            context.Adresses.Update(adress);
            if (patientId > 0)
            {
                Patient p = GetOnePatient(patientId);
                p.Adress = adress;
                UpdatePatient(p);
            }
            context.SaveChanges();
        }

        public Adress getLastAddedAdress(Adress adress)
        {
            foreach (Adress a in context.Adresses)
            {
                if (a.ToString() == adress.ToString())
                    return a;
            }
            return null;
        }

        public ICollection<FysioWorker> GetAllFysioWorkers()
        {

            List<FysioWorker> coll = new();
            foreach (FysioWorker w in context.FysioWorkers)
                coll.Add(w);
            return coll;
        }

        public FysioWorker GetOneFysioWorker(int id)
        {
            foreach (FysioWorker f in context.FysioWorkers)
                if (f.FysioWorkerID == id)
                    return f;
            return null;
        }

        public FysioWorker GetFysioWorkerByEmail(string email)
        {
            if (email.Equals("default@therapist.com"))
            {
                foreach (FysioWorker fw in GetAllFysioWorkers())
                {
                    if (!fw.IsStudent)
                        return fw;
                }
                return null;
            }
            else if (email.Equals("default@student.com"))
            {
                foreach (FysioWorker fw in GetAllFysioWorkers())
                {
                    if (fw.IsStudent)
                    {
                        return fw;
                    }
                }
                return null;
            }
            else
                foreach (FysioWorker fw in context.FysioWorkers)
                    if (fw.Email == email)
                        return fw;
            return null;
        }

        public void AddFysioWorker(FysioWorker worker)
        {
            context.FysioWorkers.Add(worker);
            context.SaveChanges();
        }

        public ICollection<Comment> GetAllComments()
        {

            List<Comment> coll = new();
            foreach (Comment c in context.Comments)
            {
                c.CommentMadeBy = GetOneFysioWorker(c.CommenterID);
                coll.Add(c);
            }
            return coll;
        }

        public Comment getOneComment(int id)
        {
            foreach (Comment c in context.Comments)
                if (c.CommentId == id)
                {
                    c.CommentMadeBy = GetOneFysioWorker(c.CommenterID);
                    return c;
                }
            return null;
        }

        public void addComment(Comment comment, int dossierID)
        {

            context.Comments.Add(comment);
            context.SaveChanges();
            AddCommentToDossier(GetLastAddedComment(comment), dossierID);
        }

        public Comment GetLastAddedComment(Comment comment)
        {
            foreach (Comment c in context.Comments)
                if (c.ToString() == comment.ToString())
                    return c;
            return null;
        }

        private void AddCommentToDossier(Comment comment, int dossierID)
        {
            foreach (PatientFile f in context.PatientFiles)
                if (f.ID == dossierID)
                {
                    f.comments.Add(comment);
                    f.CommentIDs += $" {comment.CommentId}";
                }
            context.SaveChanges();
        }

        public ICollection<Patient> GetAllPatients()
        {

            List<Patient> coll = new();
            foreach (Patient p in context.Patients)
            {
                p.Adress = GetOneAdress(p.AdressID);
                coll.Add(p);
            }
            return coll;
        }

        public Patient GetOnePatient(int id)
        {
            foreach (Patient p in context.Patients)
                if (p.PatientID == id)
                {
                    p.Adress = GetOneAdress(p.AdressID);
                    return p;
                }
            return null;
        }

        public void AddPatient(Patient patient)
        {
            context.Patients.Add(patient);
            context.SaveChanges();
        }

        public Patient GetPatientByEmail(string email)
        {
            if (email == "default@patient.com")
                foreach (Patient p in context.Patients)
                    return p;
            foreach (Patient p in context.Patients)
                if (p.Email == email)
                    return p;
            return null;
        }

        public void UpdatePatient(Patient patient, int dossierId)
        {
            UpdateAdress(patient.Adress, -1);
            context.Patients.Update(patient);
            context.SaveChanges();
            PatientFile pf = GetOnePatientFile(dossierId);
            UpdatePatientFile(pf);
        }

        public void UpdatePatient(Patient patient)
        {
            context.Patients.Update(patient);
            context.SaveChanges();
            PatientFile file = GetDossierByPatient(patient.PatientID);
            file.patient = patient;
            UpdatePatientFile(file);
        }

        public PatientFile GetDossierByPatient(int id)
        {
            foreach (PatientFile pf in context.PatientFiles)
                if (pf.patientID == id)
                {
                    context.Entry(pf)
                    .Collection(f => f.comments)
                    .Load();
                    context.Entry(pf)
                        .Collection(f => f.sessions)
                        .Load();
                    return pf;
                }
            return null;
        }

        public ICollection<TherapySession> GetAllTherapySessions()
        {

            List<TherapySession> coll = new();
            foreach (TherapySession s in context.TherapySessions)
            {
                s.SesionDoneBy = GetOneFysioWorker(s.SessionDoneByID);
                context.Add(s);
            }
            return coll;
        }

        private void RemoveSessionFromPatientFile(TherapySession session)
        {
            string sessionIDstring = "";
            PatientFile f = GetDossierBySession(session.Id);
            f.sessions.Remove(session);
            string[] sessionIds = f.sessionIDs.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in sessionIds)
                if (!s.Equals(session.Id))
                    sessionIDstring += $"{s} ";
            f.sessionIDs = sessionIDstring;
            context.PatientFiles.Update(f);
            context.SaveChanges();
        }

        public void DeleteTherapySession(TherapySession session)
        {
            RemoveSessionFromPatientFile(session);
            context.TherapySessions.Remove(session);
            context.SaveChanges();
        }

        public TherapySession GetOneTherapySession(int id)
        {
            foreach (TherapySession s in context.TherapySessions)
                if (s.Id == id)
                {
                    s.SesionDoneBy = GetOneFysioWorker(s.SessionDoneByID);
                    return s;
                }
            return null;
        }

        public void UpdateSession(TherapySession session)
        {
            context.TherapySessions.Update(session);
            context.SaveChanges();
        }

        public bool CanFitSessionInPlan(PatientFile file, DateTime startDate, int id)
        {
            bool canFit = false;
            int daysFromMonday = (int)startDate.DayOfWeek - 1;
            int daysTillSunday = 6 - daysFromMonday;
            DateTime beginWeek = startDate.AddDays((-1 * daysFromMonday));
            DateTime endWeek = startDate.AddDays(daysTillSunday);
            int sessionsInWeek = 0;
            foreach (TherapySession s in file.sessions)
            {

                if (s.Id == id)
                    continue;
                if (s.SessionStartTime > beginWeek && s.SessionStartTime < endWeek)
                    sessionsInWeek++;
            }
            if (sessionsInWeek < file.actionPlan.SessionsPerWeek)
                canFit = true;

            return canFit;
        }

        public void AddTherapySession(TherapySession session, int DossierID)
        {
            context.TherapySessions.Add(session);
            context.SaveChanges();
            AddSessionToFile(GetLastAddedSession(session), DossierID);
        }

        public List<TherapySession> GetAllTheraphySessionsByFysio(int fysioID)
        {
            List<TherapySession> sessions = new();
            foreach (TherapySession ts in context.TherapySessions)
                if (ts.SessionDoneByID == fysioID)
                    sessions.Add(ts);
            return sessions;
        }

        public TherapySession GetLastAddedSession(TherapySession session)
        {
            foreach (TherapySession s in context.TherapySessions)
                if (s.GetIDString() == session.GetIDString())
                    return s;
            return null;
        }

        private void AddSessionToFile(TherapySession session, int fileID)
        {
            foreach (PatientFile file in context.PatientFiles)
            {
                if (file.ID == fileID)
                {
                    file.sessions.Add(session);
                    file.sessionIDs += $" {session.Id}";
                    break;
                }
            }
            context.SaveChanges();
        }

        public PatientFile GetDossierByComment(int id)
        {
            string[] CommentIds;
            foreach (PatientFile file in context.PatientFiles)
            {
                CommentIds = file.CommentIDs.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in CommentIds)
                    if (s.Equals(id.ToString()))
                        return file;
            }
            return null;
        }

        public PatientFile GetDossierBySession(int id)
        {
            string[] sessionIds;
            foreach (PatientFile file in context.PatientFiles)
            {
                sessionIds = file.sessionIDs.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in sessionIds)
                    if (s.Equals(id.ToString()))
                        return file;
            }
            return null;
        }

        public ICollection<PatientFile> GetAllPatientFiles()
        {
            List<PatientFile> coll = new();
            foreach (PatientFile file in context.PatientFiles)
            {
                file.actionPlan = GetOneActionPlan(file.IdactionPlan);
                file.patient = GetOnePatient(file.patientID);
                file.mainTherapist = GetOneFysioWorker(file.IdmainTherapist);
                file.intakeDoneBy = GetOneFysioWorker(file.intakeDoneByID);
                file.intakeSuppervisedBy = GetOneFysioWorker(file.IdintakeSuppervisedBy ?? default(int));
                context.Entry(file)
                    .Collection(f => f.comments)
                    .Load();
                context.Entry(file)
                    .Collection(f => f.sessions)
                    .Load();
                coll.Add(file);
            }

            return coll;
        }

        public PatientFile GetOnePatientFile(int id)
        {
            foreach (PatientFile f in context.PatientFiles)
                if (f.ID == id)
                {
                    f.actionPlan = GetOneActionPlan(f.IdactionPlan);
                    f.patient = GetOnePatient(f.patientID);
                    f.mainTherapist = GetOneFysioWorker(f.IdmainTherapist);
                    f.intakeDoneBy = GetOneFysioWorker(f.intakeDoneByID);
                    f.intakeSuppervisedBy = GetOneFysioWorker(f.IdintakeSuppervisedBy ?? default(int));
                    context.Entry(f)
                    .Collection(file => file.comments)
                    .Load();
                    context.Entry(f)
                        .Collection(file => file.sessions)
                        .Load();
                    return f;
                }
            return null;
        }

        public void UpdatePatientFile(PatientFile patientFile)
        {
            context.PatientFiles.Update(patientFile);
            context.SaveChanges();
        }

        public void AddPatientFile(PatientFile file)
        {
            context.PatientFiles.Add(file);
            context.SaveChanges();
        }

    }
}
