using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DomainServices.Repos;
using DomainModels.Models;

namespace FysioApp.Tests
{
    public class BusinessRuleOneTests
    {
        [Fact]
        public void Maximum_Sessions_Per_Week_Reached()
        {
            var sut = new DateTime(2022, 4, 6, 13, 30, 0);
            var file = new PatientFile() 
            { 
                ActionPlan = new ActionPlan() { SessionsPerWeek = 2 },
                Sessions = new List<TherapySession>() 
                { 
                    new TherapySession() 
                    {
                        Id = 1, 
                        SessionStartTime = new DateTime(2022, 4, 7, 13, 30, 0), 
                        SessionEndTime = new DateTime(2022, 4, 7, 14, 30, 0) 
                    },
                    new TherapySession()
                    {
                        Id = 2,
                        SessionStartTime = new DateTime(2022, 4, 8, 13, 30, 0),
                        SessionEndTime = new DateTime(2022, 4, 8, 14, 30, 0)
                    }
                } 
            };
            var sessionId = 3;

            var result = new TherapySessionMock().CanFitSessionInPlan(file, sut, sessionId);

            Assert.False(result);
        }

        [Fact]
        public void One_Session_For_Week_Available()
        {
            var sut = new DateTime(2022, 4, 6, 13, 30, 0);
            var file = new PatientFile()
            {
                ActionPlan = new ActionPlan() { SessionsPerWeek = 2 },
                Sessions = new List<TherapySession>()
                {
                    new TherapySession()
                    {
                        Id = 1,
                        SessionStartTime = new DateTime(2022, 4, 7, 13, 30, 0),
                        SessionEndTime = new DateTime(2022, 4, 7, 14, 30, 0)
                    },
                }
            };
            var sessionId = 2;

            var result = new TherapySessionMock().CanFitSessionInPlan(file, sut, sessionId);

            Assert.True(result);
        }

        [Fact]
        public void Two_Sessions_For_Week_Available()
        {
            var sut = new DateTime(2022, 4, 6, 13, 30, 0);
            var file = new PatientFile()
            {
                ActionPlan = new ActionPlan() { SessionsPerWeek = 2 },
                Sessions = new List<TherapySession>()
            };
            var sessionId = 1;

            var result = new TherapySessionMock().CanFitSessionInPlan(file, sut, sessionId);

            Assert.True(result);
        }


        private class TherapySessionMock : ITherapySession
        {
            public void AddTherapySession(TherapySession therapySession) => throw new NotImplementedException();

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
            public List<TherapySession> GetAllTherapySessions() => throw new NotImplementedException();
            public TherapySession GetTherapySessionByID(int id) => throw new NotImplementedException();
            public List<TherapySession> GetTherapySessionsByFysio(int id) => throw new NotImplementedException();
            public void RemoveAllTherapySessionForFile(int id) => throw new NotImplementedException();
            public void RemovePastSessions() => throw new NotImplementedException();
            public void RemoveTherapySession(TherapySession session) => throw new NotImplementedException();
            public List<TherapySession> SortSessions(List<TherapySession> sessions) => throw new NotImplementedException();
            public bool TherapySessionExists(int id) => throw new NotImplementedException();
            public void UpdateSession(TherapySession session) => throw new NotImplementedException();
        }
    }
}