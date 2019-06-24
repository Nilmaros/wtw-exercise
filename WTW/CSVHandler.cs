using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WTW.models;
using System.IO;
using System.Linq;

namespace WTW
{
    public class CSVHandler
    {
        private readonly string _inputAddress;
        private readonly string _outputAddress;
        private readonly string _outputFileName;

        public CSVHandler(string inputAddress, string outputAddress, string outputFileName)
        {
            _inputAddress = inputAddress;
            _outputAddress = outputAddress;
            _outputFileName = outputFileName;
        }

        public List<IncrementalProduct> Read()
        {
            List<IncrementalProduct> incrementalList = new List<IncrementalProduct> ();
            int counter = 0;
            using (var reader = new StreamReader(_inputAddress))
            {
                while (!reader.EndOfStream)
                {
                    IncrementalProduct holder = new IncrementalProduct();
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (counter != 0) // Skip headers
                    {
                        holder.Product = values[0];
                        holder.OriginYear = Int32.Parse(Regex.Replace(values[1], "[^.0-9]", ""));
                        holder.DevYear = Int32.Parse(Regex.Replace(values[2], "[^.0-9]", ""));
                        holder.Value = Double.Parse(Regex.Replace(values[3], "[^.0-9]", ""));
                        incrementalList.Add(holder);
                    }
                    counter++;
                }
                return incrementalList;
            }
        }

        public void Write(string cumulativeString)
        {
            List<string> lines = cumulativeString.Split(";").ToList();
            using (var w = new StreamWriter(_outputAddress + "/" + _outputFileName + ".csv"))
            {
                foreach (string line in lines)
                {
                    w.WriteLine(line);
                    w.Flush();
                }
            }
        }
    }
}
