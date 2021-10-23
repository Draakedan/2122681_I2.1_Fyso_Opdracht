using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FysioAppUX.Controllers;
using FysioAppUX.Data;
using FysioAppUX.Components;
using DatabaseHandler.Models;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace FysioApp.Test
{
    public class HomeControllerTest
    {

        [Fact]
        public void Index_Should_Return_Index_View()
        {
            var PatientMock = new Mock<PatientRepository>();

            var sut = new HomeController(PatientMock.Object, null, null);

            PatientMock.Object.GetAll(true, new Patient("Kira", "0123456", new DateTime(1999, 12, 17)));

            var result = sut.Index() as ViewResult;

            Assert.Null(result.ViewName);
        }

        [Fact]
        public void New_Patient_Should_Contain_Correct_Age()
        {
            var PatientMock = new Mock<PatientRepository>();

            var sut = new HomeController(PatientMock.Object, null, null);

            Patient patient = new("Kira", "0123456", DateTime.Now.AddDays(-3).AddYears(-21));

            sut.NewPatient(patient);

            Assert.Equal(21, patient.Age);

        }

        [Fact]
        public void NewPatient_Should_Return_Error_When_Birthdate_Is_After_Today()
        {
            var PatientMock = new Mock<PatientRepository>();

            var sut = new HomeController(PatientMock.Object, null, null);

            Patient patient = new("Kira", "0123456", DateTime.Now.AddDays(3));

            var key = nameof(patient.Birthdate);

            sut.ViewData.ModelState.AddModelError(key, "Datum kan niet later dan vandaag");

            var result = sut.NewPatient(patient) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
            Assert.True(result.ViewData.ModelState.ContainsKey(nameof(patient.Birthdate)));
            Assert.Equal("Datum kan niet later dan vandaag", result.ViewData.ModelState[key].Errors.First().ErrorMessage);
        }

        [Fact]
        public void NewPatient_Given_Empty_Birthday_Should_Not_Add_Patient()
        {
            var PatientMock = new Mock<PatientRepository>();

            var sut = new HomeController(PatientMock.Object, null, null);

            Patient patient = new("Kira", "0123456", new DateTime());

            sut.ModelState.AddModelError(nameof(patient.Birthdate), "Birthday should not be empty");
            sut.NewPatient(patient);

            Assert.Empty(PatientMock.Object.GetAll());

        }

        //[Fact]
        //public void NewPatient_Should_Return_Error_When_Sign_Up_Date_Is_after_Today()
        //{
        //    var PatientMock = new Mock<IRepository>();

        //    var sut = new HomeController(PatientMock.Object);

        //    Patient patient = new("Kira", "0123456", DateTime.Now.AddDays(-3).AddYears(-21), DateTime.Now.AddDays(3), DateTime.Now.AddDays(6));

        //    var key = nameof(patient.RegisterDate);

        //    sut.ViewData.ModelState.AddModelError(key, "Datum kan niet later dan vandaag");

        //    var result = sut.NewPatient(patient) as ViewResult;

        //    Assert.False(result.ViewData.ModelState.IsValid);
        //    Assert.True(result.ViewData.ModelState.ContainsKey(key));
        //    Assert.Equal("Datum kan niet later dan vandaag", result.ViewData.ModelState[key].Errors.First().ErrorMessage);
        //}

        //[Fact]
        //public void NewPatien_Given_Empty_Sign_Up_Date_Should_Not_Add_Patient()
        //{
        //    var PatientMock = new Mock<IRepository>();

        //    var sut = new HomeController(PatientMock.Object);

        //    Patient patient = new("Kira", "0123456", DateTime.Now.AddYears(-21).AddDays(-3), new DateTime(), DateTime.Now);

        //    sut.ModelState.AddModelError(nameof(patient.RegisterDate), "RegisterDate should not be empty");
        //    sut.NewPatient(patient);

        //    PatientMock.Verify(PatientMock => PatientMock.Add(It.IsAny<Patient>()), Times.Never);

        //}

        //[Fact]
        //public void NewPatient_Should_Return_Error_When_FireDate_Is_Before_RegisterDate()
        //{
        //    var PatientMock = new Mock<IRepository>();

        //    var sut = new HomeController(PatientMock.Object);

        //    Patient patient = new("Kira", "0123456", DateTime.Now.AddYears(-21).AddDays(-3), DateTime.Now.AddDays(-3), DateTime.Now.AddDays(-4));

        //    var key = nameof(patient.FireDate);

        //    sut.ViewData.ModelState.AddModelError(key, "Datum ontslag kan niet voorafgaand aan de registratie datum gaan");
        //    var result = sut.NewPatient(patient) as ViewResult;

        //    Assert.False(result.ViewData.ModelState.IsValid);
        //    Assert.True(result.ViewData.ModelState.ContainsKey(key));
        //    Assert.Equal("Datum ontslag kan niet voorafgaand aan de registratie datum gaan", result.ViewData.ModelState[key].Errors.First().ErrorMessage);
        //}

        //[Fact]
        //public void NewPatient_Given_Empty_FireDate_Should_Not_Add_Patient()
        //{
        //    var PatientMock = new Mock<IRepository>();

        //    var sut = new HomeController(PatientMock.Object);

        //    Patient patient = new("Kira", "0123456", DateTime.Now.AddYears(-21).AddDays(-3), DateTime.Now.AddDays(-3), new DateTime());

        //    sut.ModelState.AddModelError(nameof(patient.FireDate), "FireDate should not be empty");
        //    sut.NewPatient(patient);

        //    PatientMock.Verify(PatientMock => PatientMock.Add(It.IsAny<Patient>()), Times.Never);
        //}

        [Fact]
        public void NewPatient_Given_All_Correct_Inputs_Should_Add_Patient()
        {
            var PatientMock = new Mock<PatientRepository>();

            var sut = new HomeController(PatientMock.Object, null, null);

            Patient patient = new("Kira", "0123456", DateTime.Now.AddYears(-21).AddDays(-3));
            sut.NewPatient(patient);

            Assert.Single(PatientMock.Object.GetAll());
        }

    }
}
