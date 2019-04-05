using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.Classes;
using System.Data.SqlClient;
using System.Data;


namespace Capstone.DAL
{
    public class ReservationsSqlDAL
    {
        private string connectionString;
        
        public ReservationsSqlDAL(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }

        private const string SQL_CreateReservation = @"insert into reservation (site_id, name, from_date, to_date) values (@siteId, @name, @fromDate, @toDate);select @@identity;";
        private const string SQL_IsSiteAvailToReserve = @"select count (site.site_id) from campground "
            + "join site on campground.campground_id = site.campground_id "
            + "left join reservation on reservation.site_id = site.site_id "
            + "where campground.campground_id = @campgroundId and reservation.from_date is null and reservation.to_date is null";
        private const string SQL_ObtainReservationDates = @"select from_date, to_date from reservation where reservation_id = @reservationId;";

        public int CreateReservation(int siteId, string name, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SQL_CreateReservation);
                    cmd.Parameters.Add("@siteId", SqlDbType.Int);
                    cmd.Parameters.Add("@name", SqlDbType.VarChar);
                    cmd.Parameters.Add("@fromDate", SqlDbType.DateTime);
                    cmd.Parameters.Add("@toDate", SqlDbType.DateTime);
                    cmd.Parameters["@siteId"].Value = siteId;
                    cmd.Parameters["@name"].Value = name;
                    cmd.Parameters["@fromDate"].Value = fromDate;
                    cmd.Parameters["@toDate"].Value = toDate;
                    cmd.Connection = connection;

                    int reservationId = Convert.ToInt32(cmd.ExecuteScalar());
                    return reservationId;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        //public decimal CalculateTotalCostOfStay(Campground camp, int reservationId)
        //{
        //    try
        //    {
        //        Reservation reserve = new Reservation();

        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            connection.Open();
        //            SqlCommand cmd = new SqlCommand(SQL_ObtainReservationDates);
        //            cmd.Parameters.Add("@reservationId", SqlDbType.Int);
        //            cmd.Parameters["@reservationId"].Value = reservationId;
        //            cmd.Connection = connection;
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            while (reader.Read())
        //            {
                        
        //                reserve.FromDate = Convert.ToDateTime(reader["from_date"]);
        //                reserve.ToDate = Convert.ToDateTime(reader["to_date"]);
        //            }

        //            double totalDays = (reserve.ToDate - reserve.FromDate).TotalDays;

        //            decimal TotalCostOfStay = camp.DailyFee * (decimal)totalDays;

        //            return TotalCostOfStay;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
    }
}
