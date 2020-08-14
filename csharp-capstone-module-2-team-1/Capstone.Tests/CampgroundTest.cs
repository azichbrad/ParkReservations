using System;
using System.Collections.Generic;
using System.Text;
using Capstone.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.Models;

namespace Capstone.Tests
{
    [TestClass]
    public class CampgroundTest : NpcampgroundTestInitialize
    {

        [TestMethod]
        public void GetCampgroundTest()
        {
            // Arrange
            CampgroundDAO campgroundDAO = new CampgroundDAO(connectionString);
            Park park = new Park()

            {
                ID = parkIdTest,
                Name = parkNameToTest,
                Location = parkLocationTest,
                EstablishedDate = parkEstablishDateTest,
                Area = areaTest,
                AnnualVisitorCount = annualVistorCountTest,
                Description = descriptionTest,
            };
                
            // Act
            IList<Campground> campground = campgroundDAO.GetCampgrounds(park);
            // Assert
            Assert.IsTrue(campground.Count > 0);
        }


    }
}
