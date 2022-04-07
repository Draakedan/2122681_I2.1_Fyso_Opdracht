using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FysioApp.Tests
{
    public class BusinessRuleSixTests
    {
        [Fact]
        public void Patient_Cant_Cancel_Session_within_Three_Hours()
        {
            var sut = DateTime.Now.AddHours(3);

            var result = DeleteSessionMock(sut);

            Assert.False(result);
        }

        [Fact]
        public void Patient_Cant_Cancel_Session_Within_TwentyFour_Hours()
        {
            var sut = DateTime.Now.AddHours(24);

            var result = DeleteSessionMock(sut);

            Assert.False(result);
        }

        [Fact]
        public void Patient_Can_Cancel_Session_Within_TwentyEight_Hours()
        {
            var sut = DateTime.Now.AddHours(28);

            var result = DeleteSessionMock(sut);

            Assert.True(result);
        }

        private static bool DeleteSessionMock(DateTime sessionTime)
        {
            return sessionTime.Subtract(DateTime.Now).TotalHours > 24;
        }
    }
}
