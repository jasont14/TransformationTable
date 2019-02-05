using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateTransformationTable
{
    class Program
    {
        static void Main(string[] args)
        {
            bool run = true;
            while (run)
            {
                Console.WriteLine("Date Transformation Table Creation\n\n");
                Console.Write("Enter Start Year: ");

                int sYear, eYear, tType;
                while (!int.TryParse(Console.ReadLine(), out sYear))
                {
                    Console.Write("\n Enter Start Year: ");
                }

                Console.Write("Enter End Year: ");

                while (!int.TryParse(Console.ReadLine(), out eYear))
                {
                    Console.Write("\n Enter End Year: ");
                }

                Console.Write("Enter Transformation Type: \n");
                Console.Write("\t1 = Month\n");
                Console.Write("\t2 = Quarter \n");
                Console.Write("\t3 = Year to Date\n");
                Console.Write("Type (1-3): ");

                while (!int.TryParse(Console.ReadLine(), out tType))
                {
                    Console.Write("\nPlease enter transformation type (1-3): ");
                }

                Console.Write("Please enter the file write location: ");
                string path = Console.ReadLine();

                DateTransformationFunctions dtf = new DateTransformationFunctions(sYear, eYear, (DateTransformationFunctions.TranformationType)tType, path);
                dtf.WriteTableToCSVFile();

                Console.Write("Write Another File (Y/N): ");
                string result = Console.ReadLine();
                if (result.ToLower() == "n")
                {
                    run = false;
                }
                else
                {
                    Console.Clear();
                }
            }
        }
    }
}
