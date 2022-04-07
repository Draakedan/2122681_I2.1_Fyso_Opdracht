using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FysioApp.Tests
{
    public class BusinessRuleFiveTests
    {
        [Fact]
        public void Patient_Cant_Be_Under_Sixteen()
        {
            var sut = DateTime.Now.AddYears(-14).AddDays(1);

            var result = CheckAgeMock(sut);

            Assert.True(result);
        }

        [Fact]
        public void Patient_Can_Be_Sixteen()
        {
            var sut = DateTime.Now.AddYears(-15).AddDays(1);

            var result = CheckAgeMock(sut);

            Assert.False(result);
        }

        [Fact]
        public void Patient_Can_Be_Over_Sixteen()
        {
            var sut = DateTime.Now.AddYears(-17).AddDays(1);

            var result = CheckAgeMock(sut);

            Assert.False(result);
        }

        private static bool CheckAgeMock(DateTime birthdate)
        {
            int age = new DateTime(DateTime.Now.Subtract(birthdate).Ticks).Year;
            return age < 16;
        }
    }
}
