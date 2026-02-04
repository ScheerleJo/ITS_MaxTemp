using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS_MaxTemp.Models
{
    enum TemperatureDataColumns {
        Sensor,
        Date,
        Time,
        Temperature
    }
    internal class TemperatureData
    {
        private string filePath;
        private string fileName;
        private List<string> rawData;

        public List<string> RawData { get { return rawData; } }

        public TemperatureData(string filePath, string fileName)
        {
            this.filePath = filePath;
            this.fileName = fileName;
            ReadFile();
        }

        private void ReadFile()
        {
            try
            {
                using (StreamReader sr = new StreamReader(filePath+ "/" + fileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        rawData.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading Data: ");
                Console.WriteLine(e.Message);
            }
        }

        private void ParseData()
        {
            foreach (var line in rawData)
            {
                var split = line.Split(' ');

            }
        }
    }
}
