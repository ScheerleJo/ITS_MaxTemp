using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;

namespace ITS_MaxTemp.Models
{
    internal class TemperatureData
    {
        private string filePath;
        private string fileName;
        private List<string> rawData = new List<string>();
        private List<DataSet> dataSets = new List<DataSet>();

        public List<string> RawData { get { return rawData; } }
        public List<DataSet> DataSets { get { return dataSets; } }

        public TemperatureData(string filePath, string fileName = "")
        {
            this.filePath = filePath;
            this.fileName = fileName;
            ReadFile();
            ParseData();
        }

        private void ReadFile()
        {
            try
            {
                if (fileName != "") filePath = Path.Combine(filePath, fileName);

                using (StreamReader sr = new StreamReader(filePath))
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
            Regex sensorPattern = new Regex("^S[0-9DB]+$");
            Regex temperaturePattern = new Regex("^[0-9]+\\.[0-9]+$");

            foreach (var line in rawData)
            {
                var split = line.Split(',');
                DataSet dataSet;
                string sensor = "";
                DateTime dateTime = DateTime.Now;
                float temperature = 0.0f;
                foreach (var data in split) 
                {
                    if (temperaturePattern.IsMatch(data))
                    { 
                        float.TryParse(data, System.Globalization.NumberStyles.Float,
                            System.Globalization.CultureInfo.InvariantCulture, out temperature);
                    }
                    else if (sensorPattern.IsMatch(data))
                    {
                        sensor = data;
                    }
                    else
                    {
                        dateTime = DateTime.Parse(data);
                    }
                }
                //Console.WriteLine($"Sensor: {sensor}, Time: {dateTime}, Temperature: {temperature}");
                dataSet = new DataSet(sensor, dateTime, temperature);
                dataSets.Add(dataSet);
            }
        }
    }
}
