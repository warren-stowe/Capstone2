using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.Classes;
using System.Data;

namespace Capstone.DAL
{
    public class SitesSqlDAL
    {
        private string connectionString;

        private string SQL_GetConflictingSites = @"select count(*) as conflicting_reservation from site
                        join reservation on site.site_id = reservation.site_id where site.campground_id = @id, site.site_id = 20 and
                        not(reservation.from_date > '@departure' or
                        reservation.to_date< '@arrival'); select @@IDENTITY;";

        private string SQL_ListSites = @"select * from site where campground_id = @id";

        private string SQL_ListAvailSites = "select * "
            + "from site where site.campground_id = @campgroundId and site.site_number not in "
            + "(select site.site_number from site join campground on campground.campground_id = site.campground_id "
            + "join reservation on site.site_id = reservation.site_id where campground.campground_id = @campgroundId "
            + "and (reservation.from_date > @departureDate or reservation.to_date< @arrivalDate) ) "
            + "group by site.site_id, site.site_number, site.max_occupancy, site.accessible, site.max_rv_length, site.utilities, site.campground_id;";
            

        public SitesSqlDAL(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }

        public List<Site> GetAvailableSites(UserReservation userReservation)
        {
            List<Site> sites = new List<Site>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(SQL_ListAvailSites);
                    cmd.Parameters.AddWithValue("@campgroundId", userReservation.CampgroundID);
                    cmd.Parameters.AddWithValue("@arrivalDate", userReservation.ArrivalDate);
                    cmd.Parameters.AddWithValue("@departureDate", userReservation.DepartureDate);
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site site = new Site();
                        site.SiteId = Convert.ToInt32(reader["site_id"]);
                        site.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        site.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        site.IsAccessible = Convert.ToBoolean(reader["accessible"]);
                        site.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
                        site.UtilitiesAreAvail = Convert.ToBoolean(reader["utilities"]);

                        sites.Add(site);
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }

            return sites;
        }
    }
}
