using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FysioApp.Models;
using Xunit;

namespace FysioApp.Test
{
    public class PatientTest
    {

        private static void AddDates(Patient p, String attributeFor, DateTime d)
        {
            try
            {
                if (attributeFor == "Birth")
                    p.Birthdate = d;
                //else if (attributeFor == "Register")
                //    p.RegisterDate = d;
                //else
                //    p.FireDate = d;
            }
            catch {
                throw;
            }
        }

        [Fact]
        public void Is_Age_Correct()
        {
            // Arrange
            Patient p = new();
            AddDates(p, "Birth", DateTime.Now.AddYears(-21).AddDays(-3));

            // Act
            int age = p.Age;

            // Assert
            Assert.Equal(21, age);
        }

        [Fact]
        public void Is_Birthdate_In_Future()
        {
            // Arrange
            Patient p = new();

            // Act
           var ex = Assert.Throws<InvalidOperationException>(() => AddDates(p, "Birth", DateTime.Now.AddYears(21).AddDays(3)));

            // Assert
            Assert.Equal("Date can't be in the fututre", ex.Message);
        }

        [Fact]
        public void Is_Birthdate_Not_In_Future()
        {
            // Arrange
            Patient p = new();
            AddDates(p, "Birth", DateTime.Now.AddYears(-21).AddDays(-3));

            // Act
            DateTime d = p.Birthdate;

            // Assert
            Assert.True(d < DateTime.Now);
        }

        [Fact]
        public void Is_Registery_Date_In_Future()
        {
            // Arrange
            Patient p = new();

            //Act
            var ex = Assert.Throws<InvalidOperationException>(() => AddDates(p, "Register", DateTime.Now.AddDays(3)));

            Assert.Equal("Date can't be in the fututre", ex.Message);
        }

        //[Fact]
        //public void Is_Registery_Date_Not_In_Future()
        //{
        //    // Arrange
        //    Patient p = new();
        //    AddDates(p, "Register", DateTime.Now.AddDays(-3));

        //    // Act
        //    DateTime d = p.RegisterDate;

        //    // Assert
        //    Assert.True(d < DateTime.Now);
        //}

        //[Fact]
        //public void Is_Fire_Date_After_Registery_Date()
        //{
        //    // Arrange
        //    Patient p = new();
        //    AddDates(p, "Register", DateTime.Now.AddDays(-3));
        //    AddDates(p, "Fire", DateTime.Now);

        //    // Act
        //    DateTime rd = p.RegisterDate;
        //    DateTime fd = p.FireDate;

        //    // Assert
        //    Assert.True(rd < fd);
        //}

        //[Fact]
        //public void Is_Fire_Date_Not_After_Registery_Date()
        //{
        //    // Arrange
        //    Patient p = new();
        //    AddDates(p, "Register", DateTime.Now.AddDays(-3));

        //    //Act
        //    var ex = Assert.Throws<InvalidOperationException>(() => AddDates(p, "Fire", DateTime.Now.AddDays(-5)));

        //    Assert.Equal("The Fire Date can't be before the Register date", ex.Message);
        //}

        [Fact]
        public void Can_Add_New_Patient()
        {
            // Arrange
            PatientRepository r = new();
            Patient p = new();
            p.Name = "Kira";
            p.PatientNumber = "0123456";
            AddDates(p, "Birth", DateTime.Now.AddYears(-21).AddDays(-3));
            AddDates(p, "Register", DateTime.Now.AddDays(-3));
            AddDates(p, "Fire", DateTime.Now);

            // Act
            r.Add(p);

            // Assert
            Assert.Equal(1, r.GetSize());
            Assert.IsType<Patient>(r.Get(0));
        }
    }
}
