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

    [TestClass]
    public class ParksSqlDALTest
    {
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Park;Integrated Security=True";
        private TransactionScope transactionScope;
        private int parkId;

        [TestInitialize]
        public void Initialize()
        {
            transactionScope = new TransactionScope();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd;
                cmd = new SqlCommand("insert into park (name, location, establish_date, area, visitors, description) "
                    + "values ('test', 'Pennsylvania', '2000-01-01', 33333, 44444, 'nothing');", connection);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("select @@identity;", connection);
                parkId = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            transactionScope.Dispose();
        }

        //Test should create a Park object when passing in a park id.
        //parkId has been established in the TestIntialize section.
        [TestMethod]
        public void CreateParkTest()
        {
            //Arrange
            ParksSqlDAL parkDal = new ParksSqlDAL(connectionString);

            //Act
            Park park = parkDal.CreatePark(parkId);


            // Assert
            Assert.AreEqual("test", park.ParkName);
            Assert.AreEqual("Pennsylvania", park.Location);
            Assert.AreEqual("nothing", park.Description);
        }

        //Test should create a dictionary with park id's & corresponding names.
        [TestMethod]
        public void GetParkNamesTest()
        {
            //Arrange
            ParksSqlDAL parkDal = new ParksSqlDAL(connectionString);
            Dictionary<int, string> output = new Dictionary<int, string>();
            Dictionary<int, string> result = new Dictionary<int, string>()
            {
                { parkId, "test" }
            };

            //Act
            output = parkDal.GetParkNames();

            //Assert
            Assert.AreEqual(result[parkId], output[parkId]);
        }
    }
}