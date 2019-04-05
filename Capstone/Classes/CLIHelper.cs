using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Capstone.Classes
{
    public static class CLIHelper
    {
        public static string GetString(int numberOfOptions)
        {
            string input = "";

            while (String.IsNullOrEmpty(input))
            {
                input = Console.ReadLine().ToUpper();
                bool isInt = int.TryParse(input, out int parsedInput);

                if (!isInt && input.Equals("Q"))
                {
                    return input;
                }
                else if ((parsedInput > 0 && parsedInput <= numberOfOptions))
                {
                    return input;
                }
                else
                {
                    Console.WriteLine($"Invalid input.  Please input 1 through {numberOfOptions} or Q to Quit.");
                    input = "";
                }
            }
            return input;
        }

        public static int GetInteger(string message, int maxOption, bool accommodateZero)
        {
            string input = "";
            int intInput = 0;
            int lowOption = 1;

            if (accommodateZero)
            {
                lowOption = 0;
            }

            while (String.IsNullOrEmpty(input))
            {
                Console.WriteLine(message);

                input = Console.ReadLine();
                bool isInt = int.TryParse(input, out intInput);

                if (!isInt || (intInput < lowOption || intInput > maxOption))
                {
                    Console.WriteLine($"Invalid input.  Please input {lowOption} through {maxOption}.");
                    input = "";
                }
            }

            return intInput;
        }

        public static DateTime GetDateTime(string message)
        {
            string input = "";
            DateTime result;

            do
            {
                Console.WriteLine(message);

                input = Console.ReadLine();

                if (DateTime.TryParse(input, out result))
                {
                    return result;
                }
                else
                {
                    Console.WriteLine("Invalid input.  Try format MM/DD/YYYY.");
                    input = "";
                }

            } while (String.IsNullOrEmpty(input));

            return result;
        }
    }
}
