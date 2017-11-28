using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using static MinaHallplatserApi.Models.DepartureModel;

namespace MinaHallplatser.Tests
{
    [TestClass]
    public class DepartureTests
    {
        [TestMethod]
        public void GetMinutesUntilTest()
        {
            string date = "2017-11-27";
            string time = "21:00";

            DateTime parsedDate = DateTime.Parse(date + " " + time);
            TimeSpan span = parsedDate - DateTime.Now;

            Assert.AreEqual(GetMinutesUntil(date, time), span.Minutes + 1);
        }
    }
}
