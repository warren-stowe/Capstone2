using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Capstone.Models;
using System.Data;
using Capstone.Classes;

namespace Capstone.DAL
{
    public class CampgroundsSqlDAL
    {
        private string connectionString;
        private const string SQL_ListAllCampgrounds = "select * from campground where park_id = @id;";
        private const string SQL_SearchInSeason = "select open_from_mm, open_to_mm from campground where campground_id = @campgroundId;";

        public CampgroundsSqlDAL(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }

        public bool SearchInSeason(UserReservation userReservation)
        {
            Campground camp = new Campground();
            int fromMonth, toMonth;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SQL_SearchInSeason);
                    cmd.Parameters.Add("@campgroundId", SqlDbType.Int);
                    cmd.Parameters["@campgroundId"].Value = userReservation.CampgroundID;
                    cmd.Connection = connection;
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        camp.OpenFromMonth = Convert.ToInt32(reader["open_from_mm"]);
                        camp.OpenToMonth = Convert.ToInt32(reader["open_to_mm"]);
                    }

                }

                fromMonth = camp.OpenFromMonth;
                toMonth = camp.OpenToMonth;

                if ((userReservation.ArrivalMonth >= fromMonth && userReservation.ArrivalMonth <= toMonth) &&
                    (userReservation.DepartureMonth >= userReservation.ArrivalMonth && userReservation.DepartureMonth <= toMonth))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Campground> ListAllCampgrounds(int parkID)
        {
            List<Campground> campgrounds = new List<Campground>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SQL_ListAllCampgrounds);
                    cmd.Parameters.AddWithValue("@id", parkID);
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Campground camp = new Campground();
                        camp.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        camp.ParkId = Convert.ToInt32(reader["park_id"]);
                        camp.CampgroundName = Convert.ToString(reader["name"]);
                        camp.OpenFromMonth = Convert.ToInt32(reader["open_from_mm"]);
                        camp.OpenToMonth = Convert.ToInt32(reader["open_to_mm"]);
                        camp.DailyFee = Convert.ToDecimal(reader["daily_fee"]);

                        campgrounds.Add(camp);
                    }

                }

            }
            catch (Exception)
            {

                throw;
            }

            return campgrounds;
        }
    }
}
