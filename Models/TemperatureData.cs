using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        private List<string> rawData = new List<string>();

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
                using (StreamReader sr = new StreamReader(filePath+ @"\" + fileName))
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
                MessageBox.Show($"Error reading Data: {e.Message}\n\nPfad: {Path.Combine(filePath, fileName)}");
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
