using System;
using WTW.models;
using System.Collections.Generic;
using System.Linq;

namespace WTW
{
    public static class Converter
    {
        public static string IncrementalToCumulativeConverter(List<IncrementalProduct> list)
        {
            // Search for all product types and store in a list
            List<string> productTypes = list.Select(x => x.Product).ToList();
            productTypes = productTypes.Distinct().ToList();
            
            double valueHolder = 0;
            int minYear = list.Min(a => a.OriginYear);
            int maxYear = list.Max(a => a.OriginYear);
            int developmentYears = (maxYear - minYear) + 1; // Can never be negative
            // First row of csv output
            string cumulativeString = minYear.ToString() + "," + developmentYears.ToString() + ";";
            // Three level FOR loops to iterate around products, then through all possible year combinations
            foreach (string currentProduct in productTypes)
            {
                for (int year = minYear; year < maxYear + 1; year++)
                {
                    for (int current = year; current < maxYear + 1; current++)
                    {
                        double value = list.Where(x => x.OriginYear == year && x.DevYear == current && x.Product == currentProduct).Sum(c => c.Value);
                        cumulativeString = String.Concat(cumulativeString, (value + valueHolder).ToString());
                        cumulativeString = String.Concat(cumulativeString, ",");
                        // For those years where value returns NULL, we wan't to keep previous value and not replace it for NULL
                        if (value > 0.1)
                        {
                            valueHolder = value + valueHolder;
                        }
                    }
                    valueHolder = 0;
                }
                cumulativeString = String.Concat(cumulativeString.Substring(0,cumulativeString.Length-1), ";");
            }
            return cumulativeString;
        }
    }
}
