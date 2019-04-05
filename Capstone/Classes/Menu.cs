using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Classes;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone.Classes
{
    public class Menu
    {
        public string MenuName { get; set; }
        const string DatabaseConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Park;Integrated Security=True";
        ParksSqlDAL parksSqlDAL = new ParksSqlDAL(DatabaseConnectionString);
        CampgroundsSqlDAL campgroundsSqlDAL = new CampgroundsSqlDAL(DatabaseConnectionString);
        SitesSqlDAL siteConnect = new SitesSqlDAL(DatabaseConnectionString);
        ReservationsSqlDAL reserve = new ReservationsSqlDAL(DatabaseConnectionString);
        private Park park;
        private Campground camp;
        private List<Campground> campgrounds;
        private List<Site> sites;

        public Menu(string menuName)
        {
            MenuName = menuName;
        }

        /// <summary>
        /// Creates the initial menu which lists the names of all the parks in the database.
        /// </summary>
        /// <returns></returns>
        public string GetParksMenu(string connectionString, ParksSqlDAL parksSqlDAL)
        {
            Dictionary<int, string> parks = parksSqlDAL.GetParkNames();

            Console.WriteLine();
            Console.WriteLine("Select a Park for Further Details");

            // Loops through the list of park names, outputs to console
            foreach (KeyValuePair<int, string> park in parks)
            {
                Console.WriteLine($"{park.Key})  {park.Value}");
            }

            Console.WriteLine("Q)  Quit");

            string parkChoice = CLIHelper.GetString(parks.Count);
            Console.Clear();

            return parkChoice;
        }

        public void ViewParksInterface()
        {
            Console.WriteLine("Welcome to the Park Reservation Database!");
            string choice = GetParksMenu(DatabaseConnectionString, parksSqlDAL);
            if (choice == "Q" || choice == "q")
            {
                Environment.Exit(0);
            }
            else
            {
                // View Parks Interface (i.e. Menu 1)
                int parkID = int.Parse(choice);
                park = parksSqlDAL.CreatePark(parkID);
                ParkInfoScreen(park);
                //At this point the methods could should end up back here.  A do-while loop
                //should use a bool (ie isRunning) to determine if the processes are finished.  If so
                //go onto to finish and exit this method.  Otherwise, go back through loop.
                Console.Clear();
            }
        }

        public void ParkInfoScreen(Park park2)
        {
            Console.WriteLine(park.ToString());
            ViewCampgroundsOrSearchReservationMenu();
            //Once ViewCampgroundOrSearchReservationMenu method is done, it should return back to here.
            //Somehow this needs to then finish to return back to the method that called it, in this case
            //the ViewParkInterface method
        }


        public void ViewCampgroundsOrSearchReservationMenu()
        {
            string parkInputView = "\nSelect a Command:\n1) View Campgrounds\n2) Search for Reservation\n"
                + "3) Return to Previous Screen";
            int commandInput = CLIHelper.GetInteger(parkInputView, 3, false);

            if (commandInput == 1)
            {
                ViewCampground();
            }
            else if (commandInput == 2)
            {
                Console.WriteLine("BONUS section not yet implemented");
            } else if (commandInput == 3)
            {
                ViewParksInterface();  //should be a return statement.  This creates recursion.
                //The return statement will take it back to method that called it--in this case ParkInfoScreen.
            }
        }

        
        public void ViewCampground ()
        {
            // User selected view campgrounds, output the current park's campgrounds
            Console.Clear();
            Console.WriteLine($"{park.ParkName} National Park Campgrounds:");
            Console.Write("ID".PadRight(6) + "Name".PadRight(35) + "Month Open".PadRight(15) + "Month Closed".PadRight(15) + "Daily Fee\n");
            campgrounds = campgroundsSqlDAL.ListAllCampgrounds(park.ParkID);
            ShowCampgrounds(campgrounds);
            SearchForCampgroundReservation();
        }

        /// <summary>
        /// Outputs the properties of a list of Campground objects
        /// </summary>
        /// <param name="campgrounds"></param>
        public void ShowCampgrounds(List<Campground> campgrounds)
        {
            for (int i = 0; i < campgrounds.Count; i++)
            {
                Console.WriteLine(campgrounds[i].ToString());
            }
        }

        public void SearchReservationOrPreviousMenu ()
        {
            string SearchReservationOrMenu = "\nSelect a Command:\n1) Search Reservation\n2) Previous Menu\n";
            int commandInput = CLIHelper.GetInteger(SearchReservationOrMenu, 2, false);

            if (commandInput == 1)
            {
                SearchForCampgroundReservation();
            }
            else if (commandInput == 2)
            {
                ViewCampgroundsOrSearchReservationMenu();
            }
        }

        public void SearchForCampgroundReservation()
        {
            string arrivalDateMessage = "\nWhat is the arrival date? (YYYY/MM/DD): ";
            string departureDateMessage = "\nWhat is the departure date? (YYYY/MM/DD): ";

            Console.WriteLine();
            //int campgroundInput = CLIHelper.GetInteger("For which campground would you like to check reservations (0 to cancel)?", campgrounds.Count, true);
            int campgroundInput = 0;
            try
            {
                Console.WriteLine("For which campground would you like to check reservations (0 to cancel)?");
                campgroundInput = int.Parse(Console.ReadLine());
            }
            catch
            {
                Quit();
            }
            

            if (campgroundInput == 0)
            {
                Quit();
            }
            else
            {
                DateTime arrivalDate = CLIHelper.GetDateTime(arrivalDateMessage);
                DateTime departureDate = CLIHelper.GetDateTime(departureDateMessage);
                UserReservation userReservation = new UserReservation(campgroundInput, arrivalDate, departureDate);
                bool isInSeason = campgroundsSqlDAL.SearchInSeason(userReservation);

                if (!isInSeason)
                {
                    Console.WriteLine($"{campgrounds[campgroundInput].CampgroundName} is not in season at the specified time frame.");
                }
                else
                {
                    SearchMatchingAvailSites(userReservation);
                }

                Console.ReadKey();
            }
        }

        public void SearchMatchingAvailSites(UserReservation userReservation)
        {
            
            sites = siteConnect.GetAvailableSites(userReservation);
            for (int i = 0; i < campgrounds.Count; i++)
            {
                if (campgrounds[i].CampgroundId == userReservation.CampgroundID)
                {
                    userReservation.CampName = campgrounds[i].CampgroundName;
                    Console.Clear();
                    Console.WriteLine($"{userReservation.CampName} has the following campsites:");
                    Console.WriteLine("Site No.".PadRight(15) + "Max Occup.".PadRight(15) + "Accessible".PadRight(15) + "Max RV Length".PadRight(25) + "Utility".PadRight(15) + "Cost");
                    ShowSites(sites, userReservation, campgrounds[i].DailyFee);
                }
            }

            Console.WriteLine();
            int siteInput = CLIHelper.GetInteger("Please choose the site number you would like to book (or 0 to cancel).", sites.Count, true);

            if (siteInput == 0)
            {
                Quit();
            }
            else
            {
                ReserveSite(userReservation);
            }
        }

        public void ReserveSite(UserReservation reservation)
        {
            bool isNotNull = true;
            string nameOfReservation;
            do
            {
                Console.WriteLine("What name should the reservation be made under?");
                nameOfReservation = Console.ReadLine();

                if (String.IsNullOrEmpty(nameOfReservation))
                {
                    isNotNull = false;
                    Console.WriteLine("You must enter a name to proceed with this reservation.");
                    Console.WriteLine();
                }
            } while (!isNotNull);
            
            int confirmationId = reserve.CreateReservation(reservation.SiteID, nameOfReservation, reservation.ArrivalDate, reservation.DepartureDate);
            Console.WriteLine($"The reservation has been made and the confirmation id is {confirmationId}.");
            Console.WriteLine("Thank you for booking with us.  Enjoy your stay!");
            Console.WriteLine();
            Console.WriteLine("Hit any key to exit this system.");
        
            Console.ReadKey();
            Environment.Exit(0);
        }

        public void ShowSites(List<Site> sites, UserReservation userReservation, decimal dailyCost)
        {
            int totalDays = (userReservation.DepartureDate - userReservation.ArrivalDate).Days;
            decimal totalCost = totalDays * dailyCost;


            for (int i = 0; i < sites.Count; i++)
            {
                Console.WriteLine(sites[i].ToString() + "$" + totalCost.ToString("#.##"));
            }
        }


        public void Quit()
        {
            Console.WriteLine("You have chosen to Quit.  Have a great day!");
            Console.WriteLine();
            Console.Clear();
            ViewParksInterface();
        }
    }
}
