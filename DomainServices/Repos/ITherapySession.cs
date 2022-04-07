using System;
using System.Collections.Generic;
using DomainModels.Models;

namespace DomainServices.Repos
{
    public interface ITherapySession
    {
        void AddTherapySession(TherapySession therapySession);
        bool TherapySessionExists(int id);
        List<TherapySession> GetAllTherapySessions();
        TherapySession GetTherapySessionByID(int id);
        void RemoveTherapySession(TherapySession session);
        void UpdateSession(TherapySession session);
        bool CanFitSessionInPlan(PatientFile file, DateTime startDate, int id);
        List<TherapySession> GetTherapySessionsByFysio(int id);
        void RemovePastSessions();
        void RemoveAllTherapySessionForFile(int id);
        List<TherapySession> SortSessions(List<TherapySession> sessions);
    }
}