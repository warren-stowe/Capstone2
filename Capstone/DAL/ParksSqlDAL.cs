using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Classes;

namespace Capstone.DAL
{
    public class ParksSqlDAL
    {
        private string connectionString;
        private const string SQL_GetParkNameID = "select name, park_id from park order by name";
        private const string SQL_GetAll = "select * from park where park_id = @id";

        public ParksSqlDAL(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }


        /// <summary>
        /// Returns a dictionary of park IDs and names.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetParkNames()
        {
            Dictionary<int, string> output = new Dictionary<int, string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = SQL_GetParkNameID;
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string park = Convert.ToString(reader["name"]);
                        int parkID = Convert.ToInt16(reader["park_id"]);

                        output.Add(parkID, park);
                    }

                }

            }
            catch (Exception e)
            {

                throw e;
            }

            return output;
        }


        /// <summary>
        /// Create a park object.
        /// </summary>
        /// <param name="parkID"></param>
        /// <returns></returns>
        public Park CreatePark(int parkID)
        {
            int id = 0;
            string parkName = "";
            string location = "";
            DateTime established = new DateTime();
            int area = 0;
            int visitors = 0;
            string description = "";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = SQL_GetAll;
                    cmd.Parameters.AddWithValue("@id", parkID);
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        id = Convert.ToInt32(reader["park_id"]);
                        parkName = Convert.ToString(reader["name"]);
                        location = Convert.ToString(reader["location"]);
                        established = Convert.ToDateTime(reader["establish_date"]);
                        area = Convert.ToInt32(reader["area"]);
                        visitors = Convert.ToInt32(reader["visitors"]);
                        description = Convert.ToString(reader["description"]);
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }

            Park park = new Park(parkID, parkName, location, established, area, visitors, description);
            return park;
        }  
    }
}
