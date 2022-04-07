using DomainModels.Models;
using DomainServices.Repos;
using FysioAppUX.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FysioApp.Tests
{
    public class BusinessRuleTwoTests
    {
        [Fact]
        public void Appointment_Cant_Be_planned_Outside_Of_Workhours_Therapist()
        {
            var sut = new HomeController(null, null, null, null, null, new FysioRepoMock(), null, new TherapyRepoMock(), null, null);
            var sessionID = 2;
            var fysioID = 3;
            var startDate = new DateTime(2022, 5, 16, 7, 30, 0);
            var endTime = new DateTime(2022, 5, 16, 9, 0, 0);

            var result = sut.CheckTime(fysioID, startDate, endTime, sessionID);

            Assert.Equal("Afspraak valt (gedeeltelijk) buiten de werktijd van de therapist", result);
        }

        [Fact]
        public void Appointment_Cant_End_After_Next_Starts()
        {
            var sut = new HomeController(null, null, null, null, null, new FysioRepoMock(), null, new TherapyRepoMock(), null, null);
            var sessionID = 2;
            var fysioID = 3;
            var startDate = new DateTime(2022, 5, 16, 12, 0, 0);
            var endTime = new DateTime(2022, 5, 16, 13, 30, 0);

            var result = sut.CheckTime(fysioID, startDate, endTime, sessionID);

            Assert.Equal("Een andere sessie staat gepland voor dat deze afgerond kan worden!", result);
        }

        [Fact]
        public void Appointment_Cant_Be_Planned_During_Other_Appointment()
        {
            var sut = new HomeController(null, null, null, null, null, new FysioRepoMock(), null, new TherapyRepoMock(), null, null);
            var sessionID = 2;
            var fysioID = 3;
            var startDate = new DateTime(2022, 5, 16, 13, 15, 0);
            var endTime = new DateTime(2022, 5, 16, 14, 15, 0);

            var result = sut.CheckTime(fysioID, startDate, endTime, sessionID);

            Assert.Equal("Start tijd zit binnen een andere afspraak", result);
        }

        [Fact]
        public void Appintment_Cant_Sart_Before_Previous_Ends()
        {
            var sut = new HomeController(null, null, null, null, null, new FysioRepoMock(), null, new TherapyRepoMock(), null, null);
            var sessionID = 2;
            var fysioID = 3;
            var startDate = new DateTime(2022, 5, 16, 14, 15, 0);
            var endTime = new DateTime(2022, 5, 16, 15, 15, 0);

            var result = sut.CheckTime(fysioID, startDate, endTime, sessionID);

            Assert.Equal("Start tijd zit binnen een andere afspraak", result);
        }

        [Fact]
        public void Appointment_Can_Be_Planned_In_Available_Time()
        {
            var sut = new HomeController(null, null, null, null, null, new FysioRepoMock(), null, new TherapyRepoMock(), null, null);
            var sessionID = 2;
            var fysioID = 3;
            var startDate = new DateTime(2022, 5, 16, 11, 15, 0);
            var endTime = new DateTime(2022, 5, 16, 12, 15, 0);

            var result = sut.CheckTime(fysioID, startDate, endTime, sessionID);

            Assert.Null(result);
        }

        private class FysioRepoMock : IFysioWorker
        {
            public void AddFysioWorker(FysioWorker fysioWorker) => throw new NotImplementedException();
            public bool FysioWorkerExists(int id) => throw new NotImplementedException();
            public List<FysioWorker> GetAllFysioWorkers() => throw new NotImplementedException();
            public FysioWorker GetFysioWorkerByEmail(string email) => throw new NotImplementedException();

            public FysioWorker GetFysioWorkerByID(int id)
            {
                return new FysioWorker()
                {
                    AvailableDays = "1",
                    DayStartTime = new DateTime(2000, 1, 1, 8, 30, 0),
                    DayEndTime = new DateTime(2000, 1, 1, 17, 0, 0)
                };
            }

            public void UpdateFysioWorker(FysioWorker worker) => throw new NotImplementedException();
        }

        private class TherapyRepoMock : ITherapySession
        {
            public void AddTherapySession(TherapySession therapySession) => throw new NotImplementedException();
            public bool CanFitSessionInPlan(PatientFile file, DateTime startDate, int id) => throw new NotImplementedException();
            public List<TherapySession> GetAllTherapySessions() => throw new NotImplementedException();
            public TherapySession GetTherapySessionByID(int id) => throw new NotImplementedException();

            public List<TherapySession> GetTherapySessionsByFysio(int id)
            {
                return new List<TherapySession>()
                {
                    new TherapySession()
                    {
                        Id=1,
                        SessionStartTime= new DateTime(2022,5 ,16, 13, 0, 0),
                        SessionEndTime = new DateTime(2022, 5, 16, 14, 30, 0)
                    }
                };
            }

            public void RemoveAllTherapySessionForFile(int id) => throw new NotImplementedException();
            public void RemovePastSessions() => throw new NotImplementedException();
            public void RemoveTherapySession(TherapySession session) => throw new NotImplementedException();
            public List<TherapySession> SortSessions(List<TherapySession> sessions) => throw new NotImplementedException();
            public bool TherapySessionExists(int id) => throw new NotImplementedException();
            public void UpdateSession(TherapySession session) => throw new NotImplementedException();
        }
    }
}