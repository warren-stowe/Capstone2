using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.DAL;
using Capstone.Models;


namespace Capstone.Classes
{
    public class UserReservation
    {
        private int _CampgroundID;
        public int CampgroundID { get { return _CampgroundID; } }

        private int _SiteID;
        public int SiteID { get { return _CampgroundID; } set { _SiteID = value; } }

        private DateTime _ArrivalDate;
        public DateTime ArrivalDate { get { return _ArrivalDate; } }

        private DateTime _DepartureDate;
        public DateTime DepartureDate { get { return _DepartureDate; } }

        public int ArrivalMonth { get { return _ArrivalDate.Month; } }
        public int DepartureMonth { get { return _DepartureDate.Month; } }

        private string _CampName;
        public string CampName { get { return _CampName; } set { _CampName = value; } }

       

        public UserReservation(int campground, DateTime arrival, DateTime departure)
        {
            _CampgroundID = campground;
            _ArrivalDate = arrival;
            _DepartureDate = departure;
          
        }
    }


}
