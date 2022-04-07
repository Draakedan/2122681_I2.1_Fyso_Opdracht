using System;
using System.Collections.Generic;
using DatabaseHandler.Data;
using DomainModels.Models;
using DomainServices.Repos;
using System.Linq;

namespace DatabaseHandler.Models
{
    public class PatientFileRepository : IPatientFile
    {
        private readonly FysioDataContext Context;

        public PatientFileRepository(FysioDataContext context) => Context = context;

        public void AddPatientFile(PatientFile patientFile)
        {
            patientFile.CommentIDs = "";
            patientFile.SessionIDs = "";
            Context.PatientFiles.Add(patientFile);
            Context.SaveChanges();
        }

        public bool PatientFileExists(int id) => GetPatientFileByID(id) != null;

        public List<PatientFile> GetAllPatientFiles()
        {
            List<PatientFile> files = new();
            foreach (PatientFile f in Context.PatientFiles)
            {
                LoadData(f);
                files.Add(f);
            }
            return files;
        }

        public PatientFile GetPatientFileByID(int id)
        {
            foreach (PatientFile p in Context.PatientFiles)
                if (p.ID == id)
                {
                    LoadData(p);
                    return p;
                }
            return null;
        }

        public void UpdatePatientFile(PatientFile file)
        {
            Context.PatientFiles.Update(file);
            Context.SaveChanges();
            return;
        }

        public List<PatientFile> GetPatientFileByFysio(int fysioID)
        {
            List<PatientFile> files = new();
            foreach (PatientFile file in Context.PatientFiles)
            {
                file.MainTherapist = new FysioWorkerRepositroy(Context).GetFysioWorkerByID(file.IdmainTherapist);
                if (file.IdmainTherapist == fysioID || file.MainTherapist.IsStudent)
                {
                    LoadData(file);
                    files.Add(file);
                }
            }
            return files;
        }

        public void AddComment(Comment comment, int id)
        {
            PatientFile file = GetPatientFileByID(id);
            if (file.ID == id)
            {
                file.Comments.Add(comment);
                file.CommentIDs += $" {comment.CommentId}";
                Context.PatientFiles.Update(file);
                Context.SaveChanges();
                return;
            }
        }

        public void AddSession(TherapySession session, int id)
        {
            PatientFile file = GetPatientFileByID(id);
            if (file.ID == id)
            {
                file.Sessions.Add(session);
                file.SessionIDs += $" {session.Id}";
                Context.PatientFiles.Update(file);
                Context.SaveChanges();
                return;
            }
        }

        public PatientFile GetPatientFileByPatient(int id)
        {
            foreach (PatientFile patientFile in Context.PatientFiles)
                if (patientFile.PatientID == id)
                {
                    LoadData(patientFile);
                    return patientFile;
                }
            return null;
        }

        public PatientFile GetPatientFileBySession(int id)
        {
            string[] sessionIds;
            foreach (PatientFile file in Context.PatientFiles)
            {
                sessionIds = file.SessionIDs.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in sessionIds)
                    if (s.Equals(id.ToString()))
                        return file;
            }
            return null;
        }

        public void RemoveSessionFromPatientFile(TherapySession session, bool isInLoop)
        {
            string sessionIDstring = "";
            PatientFile f = GetPatientFileBySession(session.Id);
            f.Sessions.Remove(session);
            string[] sessionIds = f.SessionIDs.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in sessionIds)
                if (int.Parse(s) != session.Id)
                    sessionIDstring += $"{s} ";
            f.SessionIDs = sessionIDstring;
            Context.PatientFiles.Update(f);
            if (!isInLoop)
            {
                Context.SaveChanges();
            }
        }

        private void LoadData(PatientFile p)
        {
            FysioWorkerRepositroy worker = new(Context);
            p.ActionPlan = new ActionPlanRepository(Context).GetActionPlanByID(p.IdactionPlan);
            p.Patient = new PatientRepository(Context).GetPatientByID(p.PatientID);
            p.MainTherapist = worker.GetFysioWorkerByID(p.IdmainTherapist);
            p.IntakeDoneBy = worker.GetFysioWorkerByID(p.IntakeDoneByID);
            p.IntakeSuppervisedBy = worker.GetFysioWorkerByID(p.IdintakeSuppervisedBy);
            p.Sessions = LoadTherapySessions(p);
            p.Comments = LoadComments(p);
        }

        private List<Comment> LoadComments(PatientFile p)
        {
            CommentRepositroy commentRepo = new(Context);
            List<Comment> commentList = new();

            var commentIds = p.CommentIDs.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in commentIds)
                commentList.Add(commentRepo.GetCommentByID(int.Parse(s)));

            return commentList;
        }

        private List<TherapySession> LoadTherapySessions(PatientFile p)
        {
            TherapySessionRepository sessionRepo = new(Context);
            List<TherapySession> sessionList = new();

            var sessionIds = p.SessionIDs.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in sessionIds)
                sessionList.Add(sessionRepo.GetTherapySessionByID(int.Parse(s)));

            return sessionList;
        }

        public void RemoveAllFiredPatientFiles()
        {
            TherapySessionRepository session = new(Context);
            CommentRepositroy comment = new(Context);
            foreach (PatientFile patientFile in Context.PatientFiles)
            {
                if (patientFile.FireDate != new DateTime())
                {
                    if (patientFile.FireDate > DateTime.Now)
                    {
                        session.RemoveAllTherapySessionForFile(patientFile.ID);
                        comment.RemoveAllCommentsForFile(patientFile.ID);
                        Context.PatientFiles.Remove(patientFile);
                    }
                }
            }
            Context.SaveChanges();
        }
    }
}