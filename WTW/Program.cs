using System;
using System.Configuration;
using System.Collections.Generic;
using WTW.models;

namespace WTW
{
    class Program
    {
        static void Main(string[] args)
        {
            var appSettings = ConfigurationManager.AppSettings;
            string inputAddress = "";
            string outputAddress = "";
            string outputFileName = "";
            // In the event there are any issues with the settings values, we will notify user with exception handling.
            try
            {
                inputAddress = appSettings["inputAddress"] ?? "Not Found";
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error with Input Address.");
            }

            try
            {
                outputAddress = appSettings["outputAddress"] ?? "Not Found";
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error with Output Address.");
            }

            try
            {
                outputFileName = appSettings["outputFileName"] ?? "Not Found";
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Invalid Name.");
            }

            // Program
            CSVHandler csv = new CSVHandler(inputAddress, outputAddress, outputFileName);
            List<IncrementalProduct> inputCsv = csv.Read();
            string outputString = Converter.IncrementalToCumulativeConverter(inputCsv);
            csv.Write(outputString);
        }
    }
}
