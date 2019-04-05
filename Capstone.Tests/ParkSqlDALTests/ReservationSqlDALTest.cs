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
    public class ReservationSqlDALTest
    {
        private string connectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = Park; Integrated Security = True";
        private TransactionScope transactionScope;
        private int siteId6;
        private DateTime arrival = new DateTime(2019, 6, 1);
        private DateTime departure = new DateTime(2019, 6, 10);

        [TestInitialize]
        public void Initialize()
        {
            transactionScope = new TransactionScope();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd;
                cmd = new SqlCommand("insert into site (campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities) values (7, 13, 10, 1, 30, 1);", connection);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("select @@identity", connection);
                siteId6 = Convert.ToInt32(cmd.ExecuteScalar());
                
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            transactionScope.Dispose();
        }

        //Tests whether a reservation id gets returned after creating a reservation.
        [TestMethod]
        public void CreateReservationTest()
        {
            //Arrange
            ReservationsSqlDAL reserveDal = new ReservationsSqlDAL(connectionString);
            string name2 = "testers2";
            int reserveId;


            // Act
            reserveId = reserveDal.CreateReservation(siteId6, name2, arrival, departure);
            

            //Assert
            Assert.IsNotNull(reserveId);

        }
    }
}