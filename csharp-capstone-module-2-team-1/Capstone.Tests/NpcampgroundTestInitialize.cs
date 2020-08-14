using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;

namespace Capstone.Tests
{
    public class NpcampgroundTestInitialize
    {
        protected TransactionScope transactionScope;
        protected string connectionString = @"Data Source=.\SQLExpress;Initial Catalog=npcampground;Integrated Security=true";

        //campground variables
        protected int campgroundIdTest = 0;
        protected string campgroundNameToTest = "Blackwoods";
        protected int openMonthTest = 1;
        protected int closeMonthTest = 11;
        protected decimal dailyFeeTest = 30.00M;

        //park variables
        protected int parkIdTest = 0;
        protected string parkNameToTest = "Acadia";
        protected string parkLocationTest = "Utah";
        protected DateTime parkEstablishDateTest = DateTime.Parse("1919 - 02 - 26");
        protected int areaTest = 47389;
        protected int annualVistorCountTest = 1284767;
        protected string descriptionTest = "Park";

        //reservation variables
        protected int reservationIdToTest = 0;
        protected string reservationNameTest = "Smtih Family Reservation";
        protected DateTime startDateTest = DateTime.Parse("2020 / 06 / 16");
        protected DateTime endDateToTest = DateTime.Parse("2020 / 06 / 20");
        protected DateTime dateCreatedTest = DateTime.Parse("2020 - 06 - 18");

        //site variables
        protected int siteIdTest = 0;
        protected int SiteNumberToTest = 1;
        protected int maxOccupancyTest = 6;
        protected bool isAccessbileTest = true;
        protected int maxRvLegthTest = 35;
        protected bool utilitiesTest = true;


        [TestInitialize]
        public void Initialize()
        {
            transactionScope = new TransactionScope();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //Park insert
                try
                {
                    string parkInsert = $"Insert into park VALUES('{parkNameToTest}', '{parkLocationTest}', '{parkEstablishDateTest.ToShortDateString()}', {areaTest}, {annualVistorCountTest}, '{descriptionTest}'); select scope_identity();";
                    SqlCommand command = new SqlCommand(parkInsert, connection);
                    parkIdTest = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception e)
                {

                }

                //campground insert
                try
                {
                    string campgroundInsert = $"Insert into campground VALUES({parkIdTest}, '{campgroundNameToTest}', {openMonthTest}, {closeMonthTest}, {dailyFeeTest}); select scope_identity();";
                    SqlCommand command = new SqlCommand(campgroundInsert, connection);
                    campgroundIdTest = Convert.ToInt32(command.ExecuteScalar());
                }
                catch(Exception e)
                {

                }
                //site insert
                try
                {
                    string siteInsert = $"Insert into site VALUES({campgroundIdTest}, {SiteNumberToTest}, {maxOccupancyTest}, '{isAccessbileTest}', {maxRvLegthTest}, '{utilitiesTest}'); select scope_identity();";
                    SqlCommand command = new SqlCommand(siteInsert, connection);
                    siteIdTest = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception e)
                {

                }


                //reservation insert
                try
                {
                    string reservationInsert = $"Insert into reservation VALUES({siteIdTest}, '{reservationNameTest}', '{startDateTest}', '{endDateToTest}', '{dateCreatedTest}'); select scope_identity();";
                    SqlCommand command = new SqlCommand(reservationInsert, connection);
                    reservationIdToTest = Convert.ToInt32(command.ExecuteScalar());
                }catch(Exception e)
                {

                }

                

                    connection.Close();
            }
          

   
    }
        [TestCleanup]
        public void Cleanup()
        {
            transactionScope.Dispose();
        }
    }
}