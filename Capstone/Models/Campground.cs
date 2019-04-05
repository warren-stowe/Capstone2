using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Campground
    {
        public int CampgroundId { get; set; }
        public int ParkId { get; set; }
        public string CampgroundName { get; set; }
        public int OpenFromMonth { get; set; }
        public int OpenToMonth { get; set; }
        public decimal DailyFee { get; set; }



        public override string ToString()
        {

            return CampgroundId +")".PadRight(5) + CampgroundName.PadRight(35) + OpenFromMonth.ToString().PadRight(15) + OpenToMonth.ToString().PadRight(15) +
                  "$" + DailyFee.ToString("#.##");
        }



    }
}
