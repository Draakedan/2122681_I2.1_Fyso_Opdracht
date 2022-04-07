using DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FysioApp.Tests
{
    public class BusinessRuleFourTests
    {
        [Fact]
        public void Session_Can_Be_Made_If_Toelichting_Is_Not_Needed_And_Provided()
        {
            var sut = new Treatment()
            {
                toelichting_verplicht = "nee"
            };
            var specials = "toelichting";

            var result = CheckToelichtingMock(sut, specials);

            Assert.False(result);
        }

        [Fact]
        public void Session_Can_Be_Made_If_Toelichting_Is_Not_Needed_And_Not_Provided()
        {
            var sut = new Treatment()
            {
                toelichting_verplicht = "nee"
            };
            var specials = string.Empty;

            var result = CheckToelichtingMock(sut, specials);

            Assert.False(result);
        }

        [Fact]
        public void Session_Can_Be_Made_If_Toelichting_Is_Needed_And_Provided()
        {
            var sut = new Treatment()
            {
                toelichting_verplicht = "ja"
            };
            var specials = "toelichting";

            var result = CheckToelichtingMock(sut, specials);

            Assert.False(result);
        }

        [Fact]
        public void Session_Cant_Be_Made_If_Toelichting_Is_Needed_And_Not_Provided()
        {
            var sut = new Treatment()
            {
                toelichting_verplicht = "ja"
            };
            var specials = string.Empty;

            var result = CheckToelichtingMock(sut, specials);

            Assert.True(result);
        }

        public static bool CheckToelichtingMock(Treatment t, string s)
        {
            return (t.toelichting_verplicht.ToLower() == "ja" && s == string.Empty);
        }
    }
}
