using DomainModels.Models;
using FysioAppUX.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FysioApp.Tests
{
    public class BusinessRuleThreeTests
    {
        [Fact]
        public void Session_Cant_Be_Planed_After_Fire_Date()
        {
            var sut = new HomeController(null, null, null, null, null, null, null, null, null, null);
            var startTime = DateTime.Now.AddDays(-2);
            var endTime = startTime.AddHours(1);
            var patientFile = new PatientFile()
            {
                RegisterDate = DateTime.Now.AddDays(-1),
                FireDate = DateTime.Now.AddDays(3)
            };

            var result = sut.IsWithinRegisterTime(startTime, endTime, patientFile);

            Assert.Equal("Sessie mag niet voor registratie plaatsvinden", result);
        }

        [Fact]
        public void Session_Cant_Be_Planned_Before_Register_Date()
        {
            var sut = new HomeController(null, null, null, null, null, null, null, null, null, null);
            var startTime = DateTime.Now.AddDays(4);
            var endTime = startTime.AddHours(1);
            var patientFile = new PatientFile()
            {
                RegisterDate = DateTime.Now.AddDays(-1),
                FireDate = DateTime.Now.AddDays(3)
            };

            var result = sut.IsWithinRegisterTime(startTime, endTime, patientFile);

            Assert.Equal("Sessie mag niet na ontslag plaatsvinden!", result);
        }

        [Fact]
        public void Sesson_Can_Be_Planned_During_Register_Time()
        {
            var sut = new HomeController(null, null, null, null, null, null, null, null, null, null);
            var startTime = DateTime.Now.AddDays(2);
            var endTime = startTime.AddHours(1);
            var patientFile = new PatientFile()
            {
                RegisterDate = DateTime.Now.AddDays(-1),
                FireDate = DateTime.Now.AddDays(3)
            };

            var result = sut.IsWithinRegisterTime(startTime, endTime, patientFile);

            Assert.Null(result);
        }
    }
}
