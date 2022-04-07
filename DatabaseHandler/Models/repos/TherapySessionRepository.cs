using System;
using System.Collections.Generic;
using DomainModels.Models;
using DomainServices.Repos;
using DatabaseHandler.Data;
using System.Linq;

namespace DatabaseHandler.Models
{
    public class TherapySessionRepository : ITherapySession
    {
        private readonly FysioDataContext Context;

        public TherapySessionRepository(FysioDataContext context) => Context = context;

        public void AddTherapySession(TherapySession therapySession)
        {
            Context.TherapySessions.Add(therapySession);
            Context.SaveChanges();
        }

        public bool TherapySessionExists(int id) => GetTherapySessionByID(id) != null;

        public List<TherapySession> GetAllTherapySessions()
        {
            List<TherapySession> TherapySessionList = new();
            foreach (TherapySession t in Context.TherapySessions)
            {
                t.SesionDoneBy = new FysioWorkerRepositroy(Context).GetFysioWorkerByID(t.SessionDoneByID);
                TherapySessionList.Add(t);
            }
            return TherapySessionList;
        }

        public TherapySession GetTherapySessionByID(int id)
        {
            foreach (TherapySession t in Context.TherapySessions)
                if (t.Id == id)
                {
                    t.SesionDoneBy = new FysioWorkerRepositroy(Context).GetFysioWorkerByID(t.SessionDoneByID);
                    return t;
                }
            return null;
        }

        public void RemoveTherapySession(TherapySession session)
        {
            Context.TherapySessions.Remove(session);
            Context.SaveChanges();
        }

        public void UpdateSession(TherapySession session)
        {
            Context.TherapySessions.Update(session);
            Context.SaveChanges();
        }

        public bool CanFitSessionInPlan(PatientFile file, DateTime startDate, int id)
        {
            if (file == null)
                return false;

            bool canFit = false;
            int daysFromMonday = (int)startDate.DayOfWeek - 1;
            int daysTillSunday = 6 - daysFromMonday;
            DateTime beginWeek = startDate.AddDays((-1 * daysFromMonday));
            DateTime endWeek = startDate.AddDays(daysTillSunday);
            int sessionsInWeek = 0;
            foreach (TherapySession s in file.Sessions)
            {
                if (s.Id == id)
                    continue;
                if (s.SessionStartTime > beginWeek && s.SessionStartTime < endWeek)
                    sessionsInWeek++;
            }
            if (sessionsInWeek < file.ActionPlan.SessionsPerWeek)
                canFit = true;

            return canFit;
        }

        public List<TherapySession> GetTherapySessionsByFysio(int id)
        {
            List<TherapySession> sessions = new();
            foreach (TherapySession session in Context.TherapySessions)
                if (session.SessionDoneByID == id)
                {
                    session.SesionDoneBy = new FysioWorkerRepositroy(Context).GetFysioWorkerByID(session.SessionDoneByID);
                    sessions.Add(session);
                }
            return sessions;
        }

        public void RemovePastSessions()
        {
            foreach (TherapySession session in Context.TherapySessions)
            {
                if (session.SessionEndTime < DateTime.Now)
                {
                    new PatientFileRepository(Context).RemoveSessionFromPatientFile(session, true);
                    Context.TherapySessions.Remove(session);
                }
            }
            Context.SaveChanges();
        }

        public void RemoveAllTherapySessionForFile(int id)
        {
            PatientFile patientFile = new PatientFileRepository(Context).GetPatientFileByID(id);
            foreach (TherapySession session in patientFile.Sessions)
                Context.TherapySessions.Remove(session);
        }

        public List<TherapySession> SortSessions(List<TherapySession> sessions)
        {
            List<TherapySession> sortedSessions = sessions.OrderBy(o => o.SessionStartTime).ToList();
            return sortedSessions;
        }
    }
}