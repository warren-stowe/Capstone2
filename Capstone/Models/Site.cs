using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Site
    {
        public int SiteId { get; set; }
        public int CampgroundId { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public bool IsAccessible { get; set; }
        public int MaxRVLength { get; set; }
        public bool UtilitiesAreAvail { get; set; }

        public override string ToString()
        {
            string accessibility, utilities;
            if (IsAccessible)
            { accessibility = "Yes"; } else { accessibility = "No"; }

            if (UtilitiesAreAvail)
            { utilities = "Yes"; } else { utilities = "N/A"; }

            return SiteNumber.ToString().PadRight(15) + MaxOccupancy.ToString().PadRight(15) + accessibility.PadRight(15) +
                MaxRVLength.ToString().PadRight(25) + utilities.PadRight(15);
        }
    }
}
