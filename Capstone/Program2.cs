using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Classes;
using Capstone.DAL;

namespace Capstone
{
    public class Program2
    {
        static void Main(string[] args)
        {
            Menu startMenu = new Menu("Start Menu");
            startMenu.ViewParksInterface();

            Console.WriteLine("Thank you for using the Park Reservation Database!");

            Console.ReadKey();
        }
    }
}
