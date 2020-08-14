using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone.Tests
{
    [TestClass]
    public class CapstoneCLITest
    {
        private ICampgroundDAO campgroundDAO;
        private IParkDAO parkDAO;
        private IReservationDAO reservationDAO;
        private ISiteDAO siteDAO;

        [TestMethod]
        public void VerifyDatesTest()
        {

            CapstoneCLI capstoneCLI = new CapstoneCLI(campgroundDAO, parkDAO, reservationDAO, siteDAO);

            DateTime arrival = DateTime.Parse("2020 / 06 / 16");
            DateTime departure = DateTime.Parse("2020 / 06 / 20");


            Assert.IsTrue(capstoneCLI.VerifyDates(arrival, departure));

        }

        [TestMethod]
        public void VerifyWrongWayDatesTest()
        {

            CapstoneCLI capstoneCLI = new CapstoneCLI(campgroundDAO, parkDAO, reservationDAO, siteDAO);

            DateTime arrival = DateTime.Parse("2020 / 06 / 30");
            DateTime departure = DateTime.Parse("2020 / 06 / 20");


            Assert.IsFalse(capstoneCLI.VerifyDates(arrival, departure));
        }

        [TestMethod]
        public void VerifyIntChoiceTest()
        {
            CapstoneCLI capstoneCLI = new CapstoneCLI(campgroundDAO, parkDAO, reservationDAO, siteDAO);

            int choice = 5;
            int numberOfPossibilities = 10;

            Assert.IsTrue(capstoneCLI.VerifyIntChoice(choice, numberOfPossibilities));
        }
        [TestMethod]
        public void VerifyIntChoiceTestExpectFalse()
        {
            CapstoneCLI capstoneCLI = new CapstoneCLI(campgroundDAO, parkDAO, reservationDAO, siteDAO);

            int choice = 10;
            int numberOfPossibilities = 9;

            Assert.IsFalse(capstoneCLI.VerifyIntChoice(choice, numberOfPossibilities));
        }
        [TestMethod]
        public void VerifyTotalCostTest()
        {
            CapstoneCLI capstoneCLI = new CapstoneCLI(campgroundDAO, parkDAO, reservationDAO, siteDAO);

            DateTime arrival = DateTime.Parse("2020 / 06 / 20");
            DateTime departure = DateTime.Parse("2020 / 06 / 21");
            decimal dailyFee = 10M;

            Assert.AreEqual(20M, capstoneCLI.CalculateTotalCostofStay(arrival, departure, dailyFee));
        }
      

        }
}

