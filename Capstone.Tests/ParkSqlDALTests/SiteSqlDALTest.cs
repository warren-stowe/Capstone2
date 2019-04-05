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
    public class SiteSqlDALTest
    {
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Park;Integrated Security=True";
        private TransactionScope transactionScope;
        private int siteId1, siteId2, siteId3, siteId4, siteId5;
        private DateTime arrival = new DateTime(2019, 6, 1);
        private DateTime departure = new DateTime(2019, 6, 10);
        private ReservationsSqlDAL reserve3;
        private Site site;

        [TestInitialize]
        public void Initialize()
        {
            transactionScope = new TransactionScope();

            site = new Site
            {
                CampgroundId = 13,
                SiteNumber = 10,
                MaxOccupancy = 6,
                IsAccessible = true,
                MaxRVLength = 30,
                UtilitiesAreAvail = true
            };

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd;
                cmd = new SqlCommand("insert into site (campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities) values (13, 10, 6, 1, 30, 1);", connection);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("select @@identity", connection);
                siteId1 = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("insert into site(campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities) values(7, 14, 6, 0, 25, 0);", connection);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("select @@identity", connection);
                siteId2 = Convert.ToInt32(cmd.ExecuteScalar());


                cmd = new SqlCommand("insert into site(campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities) values(7, 15, 8, 1, 20, 1);", connection);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("select @@identity", connection);
                siteId3 = Convert.ToInt32(cmd.ExecuteScalar());


                cmd = new SqlCommand("insert into site(campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities) values(7, 16, 4, 0, 0, 0);", connection);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("select @@identity", connection);
                siteId4 = Convert.ToInt32(cmd.ExecuteScalar());


                cmd = new SqlCommand("insert into site(campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities) values(7, 17, 20, 1, 50, 1);", connection);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("select @@identity", connection);
                siteId5 = Convert.ToInt32(cmd.ExecuteScalar());

                reserve3 = new ReservationsSqlDAL(connectionString);
                reserve3.CreateReservation(siteId5, "testers", arrival, departure);
                
            }

        }

        [TestCleanup]
        public void CleanUp()
        {
            transactionScope.Dispose();
        }

        //Test should return only the campgrounds that don't conflict with the reservation created in the
        //TestInitialize section.  The campgroundId was established in the TestInitialize section when inserting
        //the 5 campgrounds.
        [TestMethod]
        public void GetAvailableSitesTest()
        {
            //Arrange
            SitesSqlDAL siteDal = new SitesSqlDAL(connectionString);
            List<Site> sites = new List<Site>();
            DateTime arrival2 = new DateTime(2019, 6, 5);
            DateTime departure2 = new DateTime(2019, 6, 15);
            UserReservation reserve4 = new UserReservation(7, arrival2, departure2);

            //Act
            sites = siteDal.GetAvailableSites(reserve4);

            // Assert
            //Assert.AreEqual(13, sites[siteId1].SiteNumber);
            //Assert.AreEqual(14, sites[siteId2].SiteNumber);
            //Assert.AreEqual(15, sites[siteId3].SiteNumber);
            //Assert.AreEqual(16, sites[siteId4].SiteNumber);

            CollectionAssert.Contains(sites, site);

            

        }
    }
}