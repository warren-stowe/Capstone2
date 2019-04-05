using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Classes
{
    public class Park
    {

        public int ParkID { get; }
        public string ParkName { get; }
        public string Location { get; }
        public DateTime EstablishedDate { get; }
        public int Area { get; }
        public int Visitors { get; }
        public string Description { get; }

        public Park(int parkID, string name, string location, DateTime establishedDate, int area, int visitors, string description)
        {
            ParkID = parkID;
            ParkName = name;
            Location = location;
            EstablishedDate = establishedDate;
            Area = area;
            Visitors = visitors;
            Description = description;
        }

        public override string ToString()
        {

            string output = ParkName + "\nLocation:".PadRight(20) + Location + "\nEstablished:".PadRight(20) + EstablishedDate.ToString("MM/dd/yyyy") + 
                "\nArea:".PadRight(20) + String.Format("{0:n0}", Area) + " sq km" + "\nAnnual Visitors:".PadRight(20) + String.Format("{0:n0}", Visitors) + "\n\n" + Description;

            return output;
        }
    }
}
