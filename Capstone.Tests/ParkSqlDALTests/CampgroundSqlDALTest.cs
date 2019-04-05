//using Microsoft.Analytics.Interfaces;
//using Microsoft.Analytics.Types.Sql;
//using Microsoft.Analytics.UnitTest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.Models;
using Capstone.DAL;
using Capstone.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capstone.Tests.ParkSqlDALTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class CampgroundSqlDALTest
    {
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Park;Integrated Security=True";
        private TransactionScope transactionScope;        
        private int campId;

        [TestInitialize]
        public void Initialize()
        {
            transactionScope = new TransactionScope();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd;
                cmd = new SqlCommand("insert into campground (park_id, name, open_from_mm, open_to_mm, daily_fee)"
                    + " values (1, 'Test', 6, 9, 22.00);", connection);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("select @@identity;", connection);
                campId = Convert.ToInt32(cmd.ExecuteScalar());
                

            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            transactionScope.Dispose();
        }
         
        //TEST:  Using the "Test" Campground, list that campground.
        [TestMethod]
        public void ListCampgroundsTest()
        {
            //Arrange
            CampgroundsSqlDAL campgroundDAL = new CampgroundsSqlDAL(connectionString);

            //Act
            //parkId established in the TestInitialize section above
            List<Campground> campgrounds = campgroundDAL.ListAllCampgrounds(1);

            //Assert
            Assert.AreEqual("Test", campgrounds[campgrounds.Count-1].CampgroundName);
            Assert.AreEqual(22.00m, campgrounds[campgrounds.Count-1].DailyFee);
        }

        //TEST: Using the "Test" Campground, correctly indicate if it is in season or not in season.
        [TestMethod]
        public void SearchInSeasonTest()
        {
            // Arrange
            CampgroundsSqlDAL campgroundDAL = new CampgroundsSqlDAL(connectionString);
            DateTime arrival = new DateTime(2019, 6, 1);
            DateTime departure = new DateTime(2019, 6, 10);
            UserReservation reserve = new UserReservation(campId, arrival, departure);

            //Act
            bool isInSeason = campgroundDAL.SearchInSeason(reserve);

            //Assert
            Assert.IsTrue(isInSeason);

            //Set up a new test that should return false.
            DateTime arrival2 = new DateTime(2019, 2, 15);
            DateTime departure2 = new DateTime(2019, 2, 22);
            UserReservation reserve2 = new UserReservation(campId, arrival2, departure2);
            bool isInSeason2 = campgroundDAL.SearchInSeason(reserve2);
            Assert.IsFalse(isInSeason2);

            //add test where you put departure before arrival, etc
            //test bounds like max dates/ end dates

        }
    }
}